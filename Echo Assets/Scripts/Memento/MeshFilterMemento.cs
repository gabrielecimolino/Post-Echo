using Echo.Utility;
using UnityEditor;
using UnityEngine;

namespace Echo.Memento
{
    public class MeshFilterMemento : Memento<MeshFilter>
    {
        public Mesh m_mesh;
        
        protected override void RecordData(MeshFilter _originator)
        {
            m_mesh = _originator.sharedMesh;
        }

        public override void SetData(MeshFilter _originator)
        {
            _originator.sharedMesh = m_mesh;
        }

        protected override string GetDataString(string _format)
        {
            return AssetDatabase.GetAssetPath(m_mesh);
        }

        protected override void SetDataString(string[] _tokens)
        {
            m_mesh = AssetDatabase.LoadAssetAtPath<Mesh>(_tokens[1]);
        }
    }
}