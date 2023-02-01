using Echo.Utility;
using UnityEngine;

namespace Echo.Memento
{
    public class TransformMemento : Memento<Transform>
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        protected override void RecordData(Transform _originator)
        {
            position = _originator.position;
            rotation = _originator.rotation;
            scale = _originator.lossyScale;
        }

        public override void SetData(Transform _originator)
        {
            _originator.position = position;
            _originator.rotation = rotation;
            _originator.localScale = scale;
        }

        protected override string GetDataString(string _format)
        {
            return position.ToString(_format) + separator + rotation.ToString(_format) + separator + scale.ToString(_format);
        }

        protected override void SetDataString(string[] _tokens)
        {
            position = Utility_Functions.ParseVector3(_tokens[1]);
            rotation = Utility_Functions.ParseQuaternion(_tokens[2]);
            scale = Utility_Functions.ParseVector3(_tokens[3]);
        }
    }
}