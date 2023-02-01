using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Echo.Utility;

namespace Echo.Recording
{
    public class Recording_Object : MonoBehaviour
    {
        //--- Public Variables ---//
        public List<RecTrack.RecTrack> m_trackComponents;
        public bool m_isStatic;
        public bool m_isKeyFocusObj; // If this is true, the vis will allow for easy focus on this object

        //--- Private Variables ---//
        private Recording_Manager m_recManager;
        private string m_gameObjectName;
        private string m_uniqueID;
        
        //--- Unity Methods ---//
        private void Awake()
        {
            // Find the recording manager
            m_recManager = GameObject.FindObjectOfType<Recording_Manager>();

            // If there is no recording manager, display a warning message and deactivate this component
            if (m_recManager == null)
            {
                Debug.LogWarning("Warning: Cannot find a recording manager!");
                this.enabled = false;
                return; 
            }

            // Message the recording manager and tell it that this object now exists
            RegisterObject();
        }

        private void OnDestroy()
        {
            // If there is no recording manager, there isn't a need to unregister
            if (m_recManager == null)
                return;

            // Message the recording manager and tell it that this object no longer exists
            UnregisterObject();
        }

        //--- Messages TO The Recording Manager ---//
        public void RegisterObject()
        {
            // Contact the recording manager and tell it this object now exists
            m_recManager.RegisterObject(this);

            // Grab the current name of the attached gameobject now that it is registered
            m_gameObjectName = this.gameObject.name;
        }

        public void UnregisterObject()
        {
            // Contact the recording manager and tell it this object is being destroyed
            m_recManager.MarkObjectDoneRecording(this);
        }

        //--- Messages FROM The Recording Manager ---//
        public void SetupObject()
        {
            // Convert the track components into the relevant interface scripts
            ConvertTrackComps();
        }

        public void StartRecording(float _startTime)
        {
            // Loop through all of the tracks and tell them to start recording
            foreach (RecTrack.RecTrack track in m_trackComponents)
                track.StartRecording(_startTime);
        }

        public void UpdateRecording(float _currentTime)
        {
            // Loop through all of the tracks and tell them to update
            foreach (RecTrack.RecTrack track in m_trackComponents)
                track.UpdateRecording(_currentTime);
        }

        public void EndRecording(float _endTime)
        {
            // Loop through all of the tracks and tell them to finish recording
            foreach (RecTrack.RecTrack track in m_trackComponents)
                track.EndRecording(_endTime);

            // Unregister the object from the recording manager now
            m_recManager.MarkObjectDoneRecording(this);
        }

        public string GetAllTrackData()
        {
            // Use a stringbuilder for efficiency in concatenation
            StringBuilder builder = new StringBuilder();

            // Add object header information
            builder.AppendLine("OBJ_START~" + this.m_gameObjectName + this.m_uniqueID + "~" + this.m_isKeyFocusObj.ToString());

            // Add all of the string data from the tracks together into one set of data
            foreach (RecTrack.RecTrack track in m_trackComponents)
            {
                // First, add the track header information
                builder.AppendLine("\tTRK_START~" + track.GetTrackName());

                // Now, add all of the actual data that the track recorded
                builder.Append(track.GetData());

                // Finally, add the track footer information
                builder.AppendLine("\tTRK_END~" + track.GetTrackName());
            }

            // Add object footer information
            builder.AppendLine("OBJ_END~" + this.m_gameObjectName + this.m_uniqueID);

            // Return the compiled data
            return builder.ToString();
        }

        //--- Default Setup Methods ---//
        public void SetupDefaultStatic()
        {
            // Set this object to be static
            m_isStatic = true;

            SetupDefault();
        }

        public void SetupDefaultDynamic()
        {
            // Set this object to be dynamic
            this.m_isStatic = false;

            SetupDefault();
        }

        void SetupDefault()
        {
            // Remove any tracks currently on this object
            foreach(RecTrack.RecTrack track in AllTracks()) DestroyImmediate(track);

            // Re-init the list
            m_trackComponents = new List<RecTrack.RecTrack>();

            // Every GameObject has an active track
            RecTrack.RecTrack activeTrack = gameObject.AddRecTrack("Active");
            activeTrack.SetupDefault();
            m_trackComponents.Add(activeTrack);

            // Create a RecTrack for each recordable Component on the GameObject
            foreach (Component comp in gameObject.GetComponents<Component>())
            {
                string typeName = comp.GetType().ToString().Split('.')[^1];
                RecTrack.RecTrack track = comp.gameObject.AddRecTrack(typeName);
                if (track)
                {
                    track.SetupDefault();
                    m_trackComponents.Add(track);
                }
            }
        }

        //--- Setters ---//
        public void SetUniqueID(string _uniqueID)
        {
            // Set the unique ID for only this object, which will later be appended to its name on export
            this.m_uniqueID = _uniqueID;
        }

        //--- Getters ---//
        public string GetUniqueID()
        {
            // Return this object's unique ID
            return this.m_uniqueID;
        }

        //--- Utility Functions ---//
        private void ConvertTrackComps()
        {
            m_trackComponents = AllTracks();
        }

        List<RecTrack.RecTrack> AllTracks()
        {
            return gameObject.GetComponents<RecTrack.RecTrack>().ToList();
        }
    }
}