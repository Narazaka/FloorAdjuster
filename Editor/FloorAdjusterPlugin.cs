using nadena.dev.ndmf;
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
                if (floorAdjuster == null) return;

                var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();
                descriptor.ViewPosition += Vector3.up * floorAdjuster.Height;

                AdjustArmature(floorAdjuster, descriptor);

                UnityEngine.Object.DestroyImmediate(floorAdjuster);
            });
        }

        void AdjustArmature(FloorAdjuster floorAdjuster, VRCAvatarDescriptor descriptor)
        {
            var armature = floorAdjuster.transform;
            var hips = floorAdjuster.Hips;
            var armaturePosition = armature.position;
            var hipsPosition = hips.position;
            var armatureScale = 1 + floorAdjuster.Height / (hipsPosition.y - armaturePosition.y);
            var zDiff = (hipsPosition.z - armaturePosition.z) * (armatureScale - 1);
            armature.localScale = Vector3.one * armatureScale;
            hips.localScale = Vector3.one / armatureScale;
            descriptor.ViewPosition += Vector3.forward * zDiff;
        }
    }
}
