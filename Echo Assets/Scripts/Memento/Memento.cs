namespace Echo.Memento
{
    public abstract class Memento<T>
    {
        protected const string separator = "~";
        
        public float m_timestamp;

        public void Initialize(float _timeStamp, T _originator)
        {
            m_timestamp = _timeStamp;
            RecordData(_originator);
        }

        public void Initialize(string _dataStr)
        {
            // Split the data string
            string[] tokens = _dataStr.Split('~');

            // The first token is the timestamp so just parse the float
            m_timestamp = float.Parse(tokens[0]);

            SetDataString(tokens);
        }

        protected abstract void RecordData(T _originator);

        public abstract void SetData(T _originator);

        public string GetString(string _format)
        {
            return m_timestamp.ToString(_format) + separator + GetDataString(_format);
        }

        protected abstract string GetDataString(string _format);

        protected abstract void SetDataString(string[] _tokens);
    }
}