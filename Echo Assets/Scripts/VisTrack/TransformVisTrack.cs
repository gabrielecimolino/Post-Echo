using Echo.Memento;
using UnityEngine;

namespace Echo.VisTrack
{
    public class TransformVisTrack : VisTrack<Transform, TransformMemento>
    {
        public override void SetupDefault()
        {
            m_target = transform;
        }
    }
}