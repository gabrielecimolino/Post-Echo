using Echo.Memento;
using UnityEngine;

namespace Echo.VisTrack
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshFilterVisTrack : VisTrack<MeshFilter, MeshFilterMemento>
    {
    }
}