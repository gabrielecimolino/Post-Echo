using UnityEngine;
using UnityEngine.Assertions;
using System.Text;
using System.Collections.Generic;
using Echo.Memento;
using Echo.Recording;
using Echo.Track;

namespace Echo.RecTrack
{
    public abstract class RecTrack : Track.Track
    {
        public abstract void StartRecording(float _startTime);
        public abstract void EndRecording(float _endTime);
        public abstract void UpdateRecording(float _currentTime);
        public abstract string GetData();
    }
    
    public abstract class RecTrack<T, D> : RecTrack where T : class where D : Memento<T>, new()
    {
        //--- Public Variables ---//
        public Recording_Settings m_recordingSettings;
        public T m_target;

        //--- Private Variables ---//
        private List<D> m_dataPoints;

        public virtual void Awake()
        {
            SetupDefault();
        }

        public override string GetTrackName()
        {
            return typeof(T).ToString().Split('.')[^1];
        }
        
        public override void SetupDefault()
        {
            m_target = gameObject.GetComponent<T>();
        }

        //--- IRecordable Interfaces ---//
        public override void StartRecording(float _startTime)
        {
            // Ensure the target is set
            Assert.IsNotNull(m_target, "m_target needs to be set for the track on object [" + this.gameObject.name + "]");

            // Init the private variables
            m_dataPoints = new List<D>();

            // Record the first data point
            RecordData(GetCurrentDataPoint(_startTime));
        }

        public override void EndRecording(float _endTime)
        {
            // Record the final data point
            RecordData(GetCurrentDataPoint(_endTime));
        }

        public override void UpdateRecording(float _currentTime)
        {
            D currentDataPoint = GetCurrentDataPoint(_currentTime);
            
            // Handle the different styles of recording
            if (m_recordingSettings.m_recordingMethod == Recording_Method.On_Change)
            {
                // If the data have changed, record the current data point
                if (Changed(m_dataPoints[^1], currentDataPoint)) RecordData(currentDataPoint);
            }
            else if (m_recordingSettings.m_recordingMethod == Recording_Method.Every_X_Seconds)
            {
                // If enough time has passed, update the recording
                if (_currentTime >= m_recordingSettings.m_nextSampleTime)
                    RecordData(currentDataPoint);
            }
            else
            {
                // Always record data when doing the every frame recording
                RecordData(currentDataPoint);
            }
        }

        protected virtual bool Changed(D lastDataPoint, D currentDataPoint)
        {
            float dif = Difference(lastDataPoint, currentDataPoint);
            return dif > m_recordingSettings.m_changeMinThreshold;
        }

        protected abstract float Difference(D lastDataPoint, D currentDataPoint);

        public void RecordData(D data)
        {
            // Ensure the datapoints are setup
            Assert.IsNotNull(m_dataPoints, "m_dataPoints must be init before calling RecordData() on object [" + this.gameObject.name + "]");

            // Add the datapoint to the list
            m_dataPoints.Add(data);

            // Recalculate the next sample time
            m_recordingSettings.m_nextSampleTime = data.m_timestamp + m_recordingSettings.m_sampleTime;
        }

        protected virtual D GetCurrentDataPoint(float _currentTime)
        {
            D currentDataPoint = new D();
            currentDataPoint.Initialize(_currentTime, m_target);
            return currentDataPoint;
        }

        public override string GetData()
        {
            // Ensure the datapoints are setup
            Assert.IsNotNull(m_dataPoints, "m_dataPoints must be init before calling GetData() on object [" + this.gameObject.name + "]");

            // Use a string builder to compile the data string efficiently
            StringBuilder stringBuilder = new StringBuilder();

            // Add all of the datapoints to the string with the requested format
            foreach (D data in m_dataPoints)
                stringBuilder.AppendLine("\t\t" + data.GetString(m_recordingSettings.m_dataFormat));

            // Return the full set of data grouped together
            return stringBuilder.ToString();
        }
    }
}