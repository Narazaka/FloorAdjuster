using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace Narazaka.VRChat.FloorAdjuster
{
    public class FloorAdjuster : MonoBehaviour, IEditorOnly
    {
        [SerializeField]
        public float Height;
    }
}
