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
            EditorGUILayout.HelpBox(T.Help, MessageType.Info);

            var skeletalFloorAdjuster = target as SkeletalFloorAdjuster;
            Util.ExtraFloorAdjusterGUI(skeletalFloorAdjuster);

#if HAS_NDMF_LOCALIZATION
            nadena.dev.ndmf.ui.LanguageSwitcher.DrawImmediate();
#endif
        }

        class T
        {
            public static istring Help => new istring("This object's height will be the floor height", "このオブジェクトの高さが床の高さになります");
        }
    }
}
