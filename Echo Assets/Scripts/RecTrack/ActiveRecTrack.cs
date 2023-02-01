using Echo.Memento;
using UnityEngine;

namespace Echo.RecTrack
{
    public class ActiveRecTrack : RecTrack<GameObject, ActiveMemento>

    {
        public static string TrackName;
        public override string GetTrackName()
        {
            return "Active";
        }

        public override void SetupDefault()
        {
            m_target = gameObject;
        }

        protected override float Difference(ActiveMemento lastDataPoint, ActiveMemento currentDataPoint)
        {
            return currentDataPoint.active != lastDataPoint.active ? float.MaxValue : 0f;
        }
    }
}