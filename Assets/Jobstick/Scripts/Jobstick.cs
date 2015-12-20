using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JobstickSDK
{
    public static class Jobstick
    {
        private static JobstickController _jobstickController;
        public static JobstickController controller
        {
            get
            {
                if (_jobstickController == null)
                {
                    GameObject contollerObject = GameObject.Find("JobstickController");
                    if (contollerObject)
                    {
                        _jobstickController = contollerObject.GetComponent<JobstickController>();
                    }
                    else
                    {
                        contollerObject = new GameObject("JobstickController");
                        _jobstickController = contollerObject.AddComponent<JobstickController>();
                    }
                }

                return _jobstickController;
            }

            private set { _jobstickController = value; }
        }

        public static JobstickEvents events
        {
            get { return JobstickManager.events; }
        }


        public static JobstickAngle angles
        {
            get
            {
                return controller.GetAnglesFirstPlayer();
            }
        }

        public static void DontDestroyOnLoad()
        {
            Object.DontDestroyOnLoad(controller);
            Object.DontDestroyOnLoad(Jobstick.events);
        }

        /**
         * window to enable bluetooch
         */
        public static void RequestBluetooch()
        {
            JobstickManager.jobstick.RequestBluetooch();
        }

        public static bool IsBluetoothOn()
        {
            return JobstickManager.jobstick.IsBluetoothOn();
        }

        public static void SetMaxPlayers(int maxPlayers)
        {
            JobstickManager.jobstick.SetMaxPlayers(maxPlayers);
        }
        
    }
}
