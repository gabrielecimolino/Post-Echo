using Echo.Memento;
using UnityEngine;

namespace Echo.RecTrack
{
    public class TransformRecTrack : RecTrack<Transform, TransformMemento>
    {
        public override void SetupDefault()
        {
            m_target = transform;
        }
        
        protected override float Difference(TransformMemento lastDataPoint, TransformMemento currentDataPoint)
        {
            return Mathf.Max((currentDataPoint.position - lastDataPoint.position).magnitude,
                Quaternion.Angle(currentDataPoint.rotation, lastDataPoint.rotation),
                (currentDataPoint.scale - lastDataPoint.scale).magnitude);
        }
    }
}