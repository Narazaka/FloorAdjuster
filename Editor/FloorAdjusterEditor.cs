using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    [CustomEditor(typeof(FloorAdjuster))]
    public class FloorAdjusterEditor : UnityEditor.Editor
    {
        SerializedProperty Height;
        SerializedProperty Hips;
        FloorAdjuster FloorAdjuster;

        Transform ArmatureParent
        {
            get => FloorAdjuster.transform.parent;
        }

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
            if (Hips.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox(T.WarnHips, MessageType.Error);
            }
            if (!MaybeArmature())
            {
                EditorGUILayout.HelpBox(T.WarnArmature, MessageType.Warning);
            }
            Util.ExtraFloorAdjusterGUI(FloorAdjuster);
            if (GUILayout.Button(T.ConvertToNewMethod))
            {
                ConvertToSkeletalFloorAdjuster();
            }

#if HAS_NDMF_LOCALIZATION
            nadena.dev.ndmf.ui.LanguageSwitcher.DrawImmediate();
#endif
        }

        void OnSceneGUI()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                if (ArmatureParent == null) return;
                var point = ArmatureParent.position + Vector3.down * FloorAdjuster.Height;
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

        bool MaybeArmature()
        {
            return ArmatureParent != null && (ArmatureParent.GetComponent<Animator>() != null || ArmatureParent.GetComponent<VRCAvatarDescriptor>() != null);
        }

        void ConvertToSkeletalFloorAdjuster()
        {
            var avatarRoot = FloorAdjuster.transform.GetComponentInParent<VRCAvatarDescriptor>();
            if (avatarRoot == null)
            {
                Debug.LogError(T.NeedsAvatarDescriptor);
                return;
            }
            Util.CreateSkeletalFloorAdjuster(avatarRoot.transform, -Height.floatValue);
            Undo.DestroyObjectImmediate(FloorAdjuster);
        }

        class T
        {
            public static istring WarnHips => new istring("Hips is not set. Please set Hips.", "Hips が設定されていません。Hips を設定してください。");
            public static istring WarnArmature => new istring("Is this attached to the Armature of the avatar?", "アバターの Armature に付いていますか？");
            public static istring ConvertToNewMethod => new istring("Convert to new method (by skeleton)", "新しい方式(by skeleton)に変換する");
            public static istring NeedsAvatarDescriptor => new istring("To convert FloorAdjuster to SkeletalFloorAdjuster, a VRCAvatarDescriptor is required.", "FloorAdjuster を SkeletalFloorAdjuster に変換するには、VRCAvatarDescriptor が必要です。");
        }
    }
}
