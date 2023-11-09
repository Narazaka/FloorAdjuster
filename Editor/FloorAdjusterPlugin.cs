using nadena.dev.ndmf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;
using nadena.dev.modular_avatar.core;
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
            InPhase(BuildPhase.Resolving).BeforePlugin("nadena.dev.modular-avatar").Run(nameof(FloorAdjuster), ctx =>
            {
                var floorAdjuster = ctx.AvatarRootObject.GetComponentInChildren<FloorAdjuster>();
                if (floorAdjuster == null) return;

                var mergeArmatures = ctx.AvatarRootObject.GetComponentsInChildren<ModularAvatarMergeArmature>();
                var descriptor = ctx.AvatarRootObject.GetComponentInChildren<VRCAvatarDescriptor>();

                descriptor.ViewPosition += Vector3.up * floorAdjuster.Height;

                AdjustArmature(floorAdjuster.transform, floorAdjuster.Height);
                foreach (var mergeArmature in mergeArmatures)
                {
                    AdjustArmature(mergeArmature.transform, floorAdjuster.Height);
                }
            });
        }

        void AdjustArmature(Transform armature, float height)
        {
            var hips = armature.Find("Hips");
            var armatureScale = 1 + height / (hips.transform.position.y - armature.transform.position.y);
            armature.localScale = Vector3.one * armatureScale;
            hips.localScale = Vector3.one / armatureScale;
        }
    }
}
