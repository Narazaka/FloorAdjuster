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
        static Color s_fColor = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        static Color s_oColor = Color.white;
        Vector3[] rectBuffer = new Vector3[4];
        void OnSceneGUI()
        {
            var sfa = target as SkeletalFloorAdjuster;
            var pos = sfa.transform.position;
            rectBuffer[0] = pos + new Vector3(0.5f, 0, 0.5f);
            rectBuffer[1] = pos + new Vector3(0.5f, 0, -0.5f);
            rectBuffer[2] = pos + new Vector3(-0.5f, 0, -0.5f);
            rectBuffer[3] = pos + new Vector3(-0.5f, 0, 0.5f);
            Handles.DrawSolidRectangleWithOutline(rectBuffer, s_fColor, s_oColor);
        }
    }
}
