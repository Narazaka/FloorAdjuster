using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    public class SetupFloorAdjuster
    {
        [MenuItem("GameObject/Setup FloorAdjuster", validate = true)]
        static bool CreateValidation()
        {
            return Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInParent<VRCAvatarDescriptor>(true) != null;
        }

        [MenuItem("GameObject/Setup FloorAdjuster")]
        static void Create()
        {
            var avatarRoot = Selection.activeGameObject.GetComponentInParent<VRCAvatarDescriptor>(true);
            Util.CreateSkeletalFloorAdjuster(avatarRoot.transform, 0f);
        }
    }
}
