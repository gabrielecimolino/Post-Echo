using Echo.Memento;
using UnityEngine;

namespace Echo.RecTrack
{
    public class CameraRecTrack : RecTrack<Camera, CameraMemento>
    {
        protected override float Difference(CameraMemento lastDataPoint, CameraMemento currentDataPoint)
        {
            return Mathf.Max(Mathf.Abs(currentDataPoint.m_fov - lastDataPoint.m_fov),
                Mathf.Abs(currentDataPoint.m_clipNear - lastDataPoint.m_clipNear),
                Mathf.Abs(currentDataPoint.m_clipFar - lastDataPoint.m_clipFar));
        }
    }
}