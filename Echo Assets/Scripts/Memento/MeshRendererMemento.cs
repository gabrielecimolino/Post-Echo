using Echo.Utility;
using UnityEditor;
using UnityEngine;

namespace Echo.Memento
{
    public class MeshRendererMemento : Memento<MeshRenderer>
    {
        public Material m_material;
        public Color m_color;
        
        protected override void RecordData(MeshRenderer _originator)
        {
            m_material = _originator.sharedMaterial;
            m_color = m_material.color;
        }

        public override void SetData(MeshRenderer _originator)
        {
            _originator.sharedMaterial = m_material;
            _originator.sharedMaterial.color = m_color;
        }

        protected override string GetDataString(string _format)
        {
            return AssetDatabase.GetAssetPath(m_material) + separator + m_color.ToString(_format);
        }

        protected override void SetDataString(string[] _tokens)
        {
            m_material = AssetDatabase.LoadAssetAtPath<Material>(_tokens[1]);
            m_color = Utility_Functions.ParseColor(_tokens[2]);
        }
    }
}