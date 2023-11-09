using UnityEngine;
using VRC.SDKBase;

namespace Narazaka.VRChat.FloorAdjuster
{
    public class FloorAdjuster : MonoBehaviour, IEditorOnly
    {
        [SerializeField]
        public float Height;
        [SerializeField]
        public Transform Hips;
    }
}
