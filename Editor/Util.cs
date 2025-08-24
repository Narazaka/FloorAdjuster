using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    internal static class Util
    {
        internal static void CreateSkeletalFloorAdjuster(Transform avatarRoot, float y = 0f)
        {
            var floorAdjusterObject = new GameObject("FloorAdjuster");
            floorAdjusterObject.transform.SetParent(avatarRoot, false);
            var skeletalFloorAdjuster = floorAdjusterObject.AddComponent<SkeletalFloorAdjuster>();
            skeletalFloorAdjuster.transform.position = avatarRoot.transform.position + Vector3.up * y;
            skeletalFloorAdjuster.transform.rotation = Quaternion.identity;
            skeletalFloorAdjuster.transform.localScale = Vector3.one;
            Undo.RegisterCreatedObjectUndo(floorAdjusterObject, "Convert FloorAdjuster to SkeletalFloorAdjuster");
            EditorGUIUtility.PingObject(skeletalFloorAdjuster);
        }

        internal static IList<Component> FindFloorAdjusters(Transform avatarRoot)
        {
            var components = new List<Component>();
            components.AddRange(avatarRoot.GetComponentsInChildren<FloorAdjuster>(true));
            components.AddRange(avatarRoot.GetComponentsInChildren<SkeletalFloorAdjuster>(true));
            return components;
        }

        internal static void DestroyOtherFloorAdjusters(Component self, IList<Component> floorAdjusters)
        {
            foreach (var adjuster in floorAdjusters)
            {
                if (adjuster == self) continue;
                if (CanSafeDestroyGameObject(adjuster.gameObject))
                {
                    Undo.DestroyObjectImmediate(adjuster.gameObject);
                }
                else
                {
                    Undo.DestroyObjectImmediate(adjuster);
                }
            }
        }

        static bool CanSafeDestroyGameObject(GameObject gameObject)
        {
            if (gameObject.transform.childCount > 0) return false;
            return gameObject.GetComponents<Component>().All(c => c is Transform || c is FloorAdjuster || c is SkeletalFloorAdjuster);
        }

        internal static void ExtraFloorAdjusterGUI(Component floorAdjusterComponent)
        {
            var floorAdjusters = FindFloorAdjusters(floorAdjusterComponent.transform.GetComponentInParent<VRCAvatarDescriptor>().transform);
            if (floorAdjusters.Count > 1)
            {
                EditorGUILayout.HelpBox(T.WarnMultipleFloorAdjusters, MessageType.Error);
                if (GUILayout.Button(T.PingOthers))
                {
                    EditorGUIUtility.PingObject(floorAdjusters.Where(c => c != floorAdjusterComponent).FirstOrDefault());
                }
                if (GUILayout.Button(T.RemoveOtherFloorAdjustersButton))
                {
                    DestroyOtherFloorAdjusters(floorAdjusterComponent, floorAdjusters);
                }
            }
        }

        internal class T
        {
            public static istring WarnMultipleFloorAdjusters => new istring("There are multiple Floor Adjusters. Please merge them into one.", "Floor Adjuster が複数あります。1つにまとめてください。");
            public static istring PingOthers => new istring("Other Floor Adjusters?", "他の Floor Adjuster?");
            public static istring RemoveOtherFloorAdjustersButton => new istring("Remove Other Floor Adjusters", "他の Floor Adjuster を削除する");
        }
    }
}
