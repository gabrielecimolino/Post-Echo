using Echo.Memento;
using UnityEngine;

namespace Echo.VisTrack
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshRendererVisTrack : VisTrack<MeshRenderer, MeshRendererMemento>
    {
        
    }
}