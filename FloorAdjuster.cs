using UnityEngine;
using VRC.SDKBase;

namespace Narazaka.VRChat.FloorAdjuster
{
    [AddComponentMenu("Floor Adjuster/Floor Adjuster (by scale)")]
    public class FloorAdjuster : MonoBehaviour, IEditorOnly
    {
        [SerializeField]
        public float Height;
        [SerializeField]
        public Transform Hips;
    }
}
