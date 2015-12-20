using System;
using UnityEngine;

namespace JobstickSDK
{
    public static class JobstickManager
    {
        public static IJobstickNative jobstick 
        {
            get { return _jobstick; }
        }

        private static IJobstickNative _jobstick;
        private static JobstickEvents _events;
        private static string nameEventManager = "JobstickEventManager";

        public static JobstickEvents events
        {
            get
            {
                return GetJobstickEventManager();
            }
        }

        static JobstickManager()
        {
#if UNITY_ANDROID
            _jobstick = new JobstickAndroid();
#elif UNITY_IOS
        _jobstick = new JobstickIos();
#endif
            GetJobstickEventManager();
        }

        static JobstickEvents GetJobstickEventManager()
        {
            if (_events == null)
            {
                GameObject eventObject = GameObject.Find(nameEventManager);
                if (eventObject)
                {
                    _events = eventObject.GetComponent<JobstickEvents>();
                }
                else
                {
                    eventObject = new GameObject(nameEventManager);
                    _events = eventObject.AddComponent<JobstickEvents>();
                }
            }

            return _events;
        }

        static public JobstickAngle ParseString(string anglesString)
        {
            //format %s_%d,%d,%d,%d,%d,%d 
            JobstickAngle angles = new JobstickAngle();

            var addressAndCord = anglesString.Split('_');

            string address = addressAndCord[0];

            var valus = addressAndCord[1].Split(',');

            angles.ax = GetValue(valus[0]);
            angles.ay = GetValue(valus[1]);
            angles.az = GetValue(valus[2]);
            angles.gx = GetValue(valus[3]);
            angles.gy = GetValue(valus[4]);
            angles.gz = GetValue(valus[5]);

            return angles;
        }

        static public int GetValue(string value)
        {
            int outInt = 0;
            Int32.TryParse(value, out outInt);
            return outInt;
        }
    }
}
