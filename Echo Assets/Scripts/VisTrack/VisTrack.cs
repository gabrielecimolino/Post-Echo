using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Echo.Track;
using Echo.Memento;
using Echo.Utility;

namespace Echo.VisTrack
{
    public abstract class VisTrack : Track.Track
    {
        public abstract bool InitWithString(string _data);
        public abstract void StartVisualization(float _startTime);
        public abstract void UpdateVisualization(float _time);
        public abstract float GetFirstTimestamp();
        public abstract float GetLastTimestamp();
    }

    public abstract class VisTrack<T,D> : VisTrack where T : class where D : Memento<T>, new()
    {
        
        //--- Private Variables ---//
        public T m_target;
        protected List<D> m_dataPoints;

        public override string GetTrackName()
        {
            return typeof(T).ToString().Split('.')[^1];
        }

        public override void SetupDefault()
        {
            m_target = gameObject.GetComponent<T>();
        }

        //--- IVisualizable Interface ---// 
        public override bool InitWithString(string _data)
        {
            try
            {
                // Create a list of data points by parsing the string
                m_dataPoints = ParseDataList(_data);

                // If everything worked correctly, return true
                return true;
            }
            catch (Exception _e)
            {
                // If something went wrong, output an error and return false
                Debug.LogError("Error in InitWithString(): " + _e.Message);
                return false;
            }
        }

        List<D> ParseDataList(string _data)
        {
            List<D> dataList = new List<D>();

            foreach (string point in _data.Split('\n'))
            {
                if (point != "")
                {
                    D dataPoint = new D();
                    dataPoint.Initialize(point);
                    dataList.Add(dataPoint);
                }
            }

            return dataList;
        }

        public override void StartVisualization(float _startTime)
        {
            if(m_target == null) SetupDefault();
            
            // Apply the initial visualization
            UpdateVisualization(_startTime);
        }

        public override void UpdateVisualization(float _time)
        {
            // Get the data point before the given time
            m_dataPoints[FindDataPointForTime(_time)].SetData(m_target);
        }

        public int FindDataPointForTime(float _time)
        {
            // Ensure the datapoints are actually setup
            Assert.IsNotNull(m_dataPoints, "m_dataPoints has to be setup for before looking for a data point on object [" + this.gameObject.name + "]");
            Assert.IsTrue(m_dataPoints.Count >= 1, "m_dataPoints cannot be empty on object [" + this.gameObject.name + "]");

            // Start by setting the selected index to 0 in case there is only one point
            int selectedIndex = 0;

            // Loop through all of the data and find the nearest point BEFORE the given time
            for (selectedIndex = 0; selectedIndex < m_dataPoints.Count - 1; selectedIndex++)
            {
                // Get the datapoint at the current index and next index
                var thisDataPoint = m_dataPoints[selectedIndex];
                var nextDataPoint = m_dataPoints[selectedIndex + 1];

                // If this datapoint is BEFORE OR AT the time and the next one is AFTER the time, then we are at the right data point
                if (thisDataPoint.m_timestamp <= _time && nextDataPoint.m_timestamp > _time)
                    break;
            }

            // Return the selected index
            return selectedIndex;
        }

        public override float GetFirstTimestamp()
        {
            // Ensure the datapoints are actually setup
            Assert.IsNotNull(m_dataPoints, "m_dataPoints has to be setup for before looking for a data point on object [" + this.gameObject.name + "]");
            Assert.IsTrue(m_dataPoints.Count >= 1, "m_dataPoints cannot be empty on object [" + this.gameObject.name + "]");

            // Return the timestamp for the first data point
            return m_dataPoints[0].m_timestamp;
        }

        public override float GetLastTimestamp()
        {
            // Ensure the datapoints are actually setup
            Assert.IsNotNull(m_dataPoints, "m_dataPoints has to be setup for before looking for a data point on object [" + this.gameObject.name + "]");
            Assert.IsTrue(m_dataPoints.Count >= 1, "m_dataPoints cannot be empty on object [" + this.gameObject.name + "]");

            // Return the timestamp for the last data point
            return m_dataPoints[^1].m_timestamp;
        }
    }
}