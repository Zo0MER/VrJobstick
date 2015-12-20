using UnityEngine;
using System.Collections;

namespace JobstickSDK
{
    public static class JobstickUniversal
    {
        public enum Mode
        {
            External,
            Auto,
            Internal,
        }

        public enum SensorType
        {
            Accelerometer,
            Gyroscope,
            Jobstick,
        }

        public static Vector3 gyroscopeUniversal
        {
            get { return universalEvent.gyroscopeAngle; }
        }

        public static Vector3 accelerometrUniversal
        {
            get { return universalEvent.accelerometrAngle; }
        }

        private static JobstickUniversalEvent _universalEvent;
        public static JobstickUniversalEvent universalEvent
        {
            get
            {
                if (_universalEvent == null)
                {
                    GameObject contollerObject = GameObject.Find("JobstickUniversalEvent");
                    if (contollerObject)
                    {
                        _universalEvent = contollerObject.GetComponent<JobstickUniversalEvent>();
                    }
                    else
                    {
                        contollerObject = new GameObject("JobstickUniversalEvent");
                        _universalEvent = contollerObject.AddComponent<JobstickUniversalEvent>();
                    }
                }

                return _universalEvent;
            }
        }

        private static JobstickUniversal.Mode _mode = JobstickUniversal.Mode.External;
        public static JobstickUniversal.Mode mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                universalEvent.mode = _mode;
            }
        }

        private static JobstickUniversal.SensorType _type = JobstickUniversal.SensorType.Accelerometer;
        public static JobstickUniversal.SensorType type
        {
            get { return _type; }
            set { _type = value; }
        }

        public static bool useComplementaryFilter
        {
            get { return universalEvent.useComplementaryFilter; }
            set { universalEvent.useComplementaryFilter = value; }
        }

        static JobstickUniversal()
        {
            mode = Mode.Auto;
            type = SensorType.Accelerometer;
        }

        public static void DontDestroyOnLoad()
        {
            Object.DontDestroyOnLoad(universalEvent);

            Jobstick.controller.StopSearchDevices();
        }

        public static bool JobstickIsConnect()
        {
            return Jobstick.controller.GetCountPlayers() > 0;
        }
    }
}
