using Echo.Memento;
using UnityEngine;

namespace Echo.RecTrack
{
    public class MeshRendererRecTrack : RecTrack<MeshRenderer, MeshRendererMemento>
    {
        protected override float Difference(MeshRendererMemento lastDataPoint, MeshRendererMemento currentDataPoint)
        {
            return (currentDataPoint.m_material != lastDataPoint.m_material || currentDataPoint.m_color != lastDataPoint.m_color) ? float.MaxValue : 0f;
        }
    }
}