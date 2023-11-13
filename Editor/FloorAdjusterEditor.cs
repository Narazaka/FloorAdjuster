using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    [CustomEditor(typeof(FloorAdjuster))]
    public class FloorAdjusterEditor : UnityEditor.Editor
    {
        SerializedProperty Height;
        SerializedProperty Hips;
        FloorAdjuster FloorAdjuster;

        void OnEnable()
        {
            Height = serializedObject.FindProperty(nameof(FloorAdjuster.Height));
            Hips = serializedObject.FindProperty(nameof(FloorAdjuster.Hips));
            FloorAdjuster = target as FloorAdjuster;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(Height);
            EditorGUILayout.PropertyField(Hips);
            if (Hips.objectReferenceValue == null)
            {
                Hips.objectReferenceValue = FloorAdjuster.transform.Find("Hips");
            }
            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                var parent = FloorAdjuster.transform.parent;
                if (parent == null) return;
                var point = parent.position + Vector3.down * FloorAdjuster.Height;
                Handles.DrawSolidRectangleWithOutline(new Vector3[] { point + new Vector3(0.5f, 0, 0.5f), point + new Vector3(0.5f, 0, -0.5f), point + new Vector3(-0.5f, 0, -0.5f), point + new Vector3(-0.5f, 0, 0.5f) }, new Color(0.5f, 0.5f, 0.5f, 0.1f), Color.white);
                var newHeight = -Handles.Slider(point, Vector3.up).y;
                if (change.changed)
                {
                    serializedObject.Update();
                    Height.floatValue = newHeight;
                    serializedObject.ApplyModifiedProperties();
                }
            }

        }
    }
}
