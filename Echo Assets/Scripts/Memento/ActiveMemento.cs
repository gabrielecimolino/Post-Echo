using UnityEngine;

namespace Echo.Memento
{
    public class ActiveMemento : Memento<GameObject>
    {
        public bool active;

        protected override void RecordData(GameObject _originator)
        {
            active = _originator.activeInHierarchy;
        }

        public override void SetData(GameObject _originator)
        {
            _originator.SetActive(active);
        }

        protected override string GetDataString(string _format)
        {
            return active.ToString();
        }

        protected override void SetDataString(string[] _tokens)
        {
            active = bool.Parse(_tokens[1]);
        }
    }
}