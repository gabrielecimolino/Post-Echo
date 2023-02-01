using Echo.Memento;
using UnityEngine;

namespace Echo.RecTrack
{
    public class MeshFilterRecTrack : RecTrack<MeshFilter, MeshFilterMemento>
    {
        protected override float Difference(MeshFilterMemento lastDataPoint, MeshFilterMemento currentDataPoint)
        {
            return currentDataPoint.m_mesh != lastDataPoint.m_mesh ? float.MaxValue : 0f;
        }
    }
}