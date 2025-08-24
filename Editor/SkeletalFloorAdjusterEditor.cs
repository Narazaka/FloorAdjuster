using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    [CustomEditor(typeof(SkeletalFloorAdjuster))]
    public class SkeletalFloorAdjusterEditor : UnityEditor.Editor
    {
        static Color faceColor = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        static Color outlineColor = Color.white;
        Vector3[] rectBuffer = new Vector3[4];

        void OnSceneGUI()
        {
            var sfa = target as SkeletalFloorAdjuster;
            var pos = sfa.transform.position;
            rectBuffer[0] = pos + new Vector3(0.5f, 0, 0.5f);
            rectBuffer[1] = pos + new Vector3(0.5f, 0, -0.5f);
            rectBuffer[2] = pos + new Vector3(-0.5f, 0, -0.5f);
            rectBuffer[3] = pos + new Vector3(-0.5f, 0, 0.5f);
            Handles.DrawSolidRectangleWithOutline(rectBuffer, faceColor, outlineColor);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("このオブジェクトの高さが床の高さになります", MessageType.Info);

            var skeletalFloorAdjuster = target as SkeletalFloorAdjuster;
            var floorAdjusters = Util.FindFloorAdjusters(skeletalFloorAdjuster.transform.GetComponentInParent<VRCAvatarDescriptor>().transform);
            if (floorAdjusters.Count > 1)
            {
                EditorGUILayout.HelpBox("Floor Adjuster が複数あります。1つにまとめてください。", MessageType.Error);
                if (GUILayout.Button("他の Floor Adjuster を削除する"))
                {
                    Util.DestroyOtherFloorAdjusters(skeletalFloorAdjuster, floorAdjusters);
                }
            }
        }
    }
}
