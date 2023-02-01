using UnityEngine;

namespace Echo.Memento
{
    public class CameraMemento : Memento<Camera>
    {
        public float m_fov;
        public float m_clipNear;
        public float m_clipFar;
        
        protected override void RecordData(Camera _originator)
        {
            m_fov = _originator.fieldOfView;
            m_clipNear = _originator.nearClipPlane;
            m_clipFar = _originator.farClipPlane;
        }

        public override void SetData(Camera _originator)
        {
            _originator.fieldOfView = m_fov;
            _originator.nearClipPlane = m_clipNear;
            _originator.farClipPlane = m_clipFar;
        }

        protected override string GetDataString(string _format)
        {
            return m_fov.ToString(_format) + separator + m_clipNear.ToString(_format) + separator + m_clipFar.ToString(_format);
        }

        protected override void SetDataString(string[] _tokens)
        {
            m_fov = float.Parse(_tokens[1]);
            m_clipNear = float.Parse(_tokens[2]);
            m_clipFar = float.Parse(_tokens[3]);
        }
    }
}