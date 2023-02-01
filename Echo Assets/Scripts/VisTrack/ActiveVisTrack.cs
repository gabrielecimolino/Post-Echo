using Echo.Memento;
using UnityEngine;

namespace Echo.VisTrack
{
    public class ActiveVisTrack : VisTrack<GameObject, ActiveMemento>
    {
        public override void SetupDefault()
        {
            m_target = gameObject;
        }
    }
}