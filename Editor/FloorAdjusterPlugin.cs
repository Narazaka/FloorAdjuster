using nadena.dev.ndmf;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

[assembly: ExportsPlugin(typeof(Narazaka.VRChat.FloorAdjuster.Editor.FloorAdjusterPlugin))]

namespace Narazaka.VRChat.FloorAdjuster.Editor
{
    public class FloorAdjusterPlugin : Plugin<FloorAdjusterPlugin>
    {
        public override string QualifiedName => "net.narazaka.vrchat.floor_adjuster";

        /// <summary>
        /// The plugin name shown in debug UIs. If not set, the qualified name will be shown.
        /// </summary>
        public override string DisplayName => nameof(FloorAdjuster);
        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming).AfterPlugin("nadena.dev.modular-avatar").Run(nameof(FloorAdjuster), ctx =>
            {
                var floorAdjuster = ctx.AvatarRootObject.GetComponentInChildren<FloorAdjuster>();
                if (floorAdjuster == null) return;

                AdjustArmature(floorAdjuster);

                var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();
                descriptor.ViewPosition += Vector3.up * floorAdjuster.Height;

                UnityEngine.Object.DestroyImmediate(floorAdjuster);
            });
        }

        void AdjustArmature(FloorAdjuster floorAdjuster)
        {
            var armature = floorAdjuster.transform;
            var hips = armature.Find("Hips");
            var armaturePosition = armature.position;
            var hipsPosition = hips.position;
            var armatureScale = 1 + floorAdjuster.Height / (hipsPosition.y - armaturePosition.y);
            armature.localScale = Vector3.one * armatureScale;
            hips.localScale = Vector3.one / armatureScale;
        }
    }
}
