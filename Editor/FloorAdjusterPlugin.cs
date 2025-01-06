using System;
using System.Linq;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

[assembly: ExportsPlugin(typeof(Narazaka.VRChat.FloorAdjuster.Editor.FloorAdjusterPlugin))]

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    public class FloorAdjusterPlugin : Plugin<FloorAdjusterPlugin>
    {
        public override string QualifiedName => "net.narazaka.vrchat.floor_adjuster";

        public override string DisplayName => nameof(FloorAdjuster);

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming).AfterPlugin("nadena.dev.modular-avatar").Run(nameof(FloorAdjuster), ctx =>
            {
                var floorAdjuster = ctx.AvatarRootObject.GetComponentInChildren<FloorAdjuster>();
                if (floorAdjuster != null) { RunFloorAdjuster(ctx, floorAdjuster); return; }

                var skeletalFloorAdjuster = ctx.AvatarRootObject.GetComponentInChildren<SkeletalFloorAdjuster>();
                if (skeletalFloorAdjuster != null) { RunSkeletalFloorAdjuster(ctx, skeletalFloorAdjuster); return; }
            });
        }

        private void RunFloorAdjuster(BuildContext ctx, FloorAdjuster floorAdjuster)
        {
            var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();
            descriptor.ViewPosition += Vector3.up * floorAdjuster.Height;

            AdjustArmature(floorAdjuster, descriptor);

            UnityEngine.Object.DestroyImmediate(floorAdjuster);
        }

        void AdjustArmature(FloorAdjuster floorAdjuster, VRCAvatarDescriptor descriptor)
        {
            var armature = floorAdjuster.transform;
            var hips = floorAdjuster.Hips;
            var armaturePosition = armature.position;
            var hipsPosition = hips.position;
            var armatureScale = 1 + floorAdjuster.Height / (hipsPosition.y - armaturePosition.y);
            var zDiff = (hipsPosition.z - armaturePosition.z) * (armatureScale - 1);
            armature.localScale = armature.localScale * armatureScale;
            hips.localScale = hips.localScale / armatureScale;
            descriptor.ViewPosition += Vector3.forward * zDiff;
        }


        private static void RunSkeletalFloorAdjuster(BuildContext ctx, SkeletalFloorAdjuster skeletalFloorAdjuster)
        {
            var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();
            var animator = ctx.AvatarRootObject.GetComponent<Animator>();

            var floorDiff = -1 * skeletalFloorAdjuster.transform.position.y;


            var adjustAvatar = UnityEngine.Object.Instantiate(animator.avatar);
            var sAdjustTargetAvatar = new SerializedObject(adjustAvatar);//AvatarBuilder から作り直す実装もしてみたけど、うまくできる方法が全く分からなかった。

            //アバターのルートモーションの大きさを変えることで Humanoid としての初期位置を変えることで高さが変わる。
            var avatarScale = sAdjustTargetAvatar.FindProperty("m_Avatar.m_Human.data.m_Scale");
            avatarScale.floatValue += floorDiff;

            //ビューポジションも IKPose か TPose の影響を受けるのでその影響の時のために補正。
            //IKPose も TPose もデフォルトの物であれば真上にしかルート移動が存在しないの Y 軸のみの補正で問題がない。
            descriptor.ViewPosition += Vector3.up * floorDiff;


            sAdjustTargetAvatar.ApplyModifiedProperties();

            animator.avatar = adjustAvatar;
            adjustAvatar.name += "-SkeletalFloorAdjusted";

            AssetDatabase.AddObjectToAsset(adjustAvatar, ctx.AssetContainer);
            UnityEngine.Object.DestroyImmediate(skeletalFloorAdjuster);
        }

    }
}
