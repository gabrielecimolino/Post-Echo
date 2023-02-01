using Echo.Memento;
using UnityEngine;

namespace Echo.VisTrack
{
    [RequireComponent(typeof(Camera))]
    public class CameraVisTrack : VisTrack<Camera, CameraMemento>
    {
    }
}