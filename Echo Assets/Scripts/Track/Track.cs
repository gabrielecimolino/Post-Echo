using UnityEngine;

namespace Echo.Track
{
    public abstract class Track : MonoBehaviour
    {
        public abstract string GetTrackName();
        public abstract void SetupDefault();
    }
}