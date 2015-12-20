using System;
using UnityEngine;

namespace JobstickSDK
{
    public class JobstickEvents : MonoBehaviour
    {
        public  event Action jobstickOnBluetoothLENoFind;
        public  event Action jobstickOnDisconnectToDevice;
        public  event Action jobstickOnNoDataFromDevice;
        public  event Action<bool> jobstickOnRequestBluetooch;
        public  event Action<string,JobstickAngle> jobstickOnAnglesFromPlugin;
        public  event Action jobstickOnConnectToDevice;

        void OnNoDataFromDevice()
        {
            if (jobstickOnNoDataFromDevice != null)
                jobstickOnNoDataFromDevice();
        }

        void OnBluetoothLENoFind()
        {
            if (jobstickOnBluetoothLENoFind != null)
                jobstickOnBluetoothLENoFind();
        }


        //it is better not to use this event,
        //it is better JobstickController.OnConnectPlayerEvent
        void OnConnectToDevice()
        {
            Debug.Log("OnConnectToDevice");

            if (jobstickOnConnectToDevice != null)
                jobstickOnConnectToDevice();
        }

        void OnDisconnectToDevice()
        {
            if (jobstickOnDisconnectToDevice != null)
                jobstickOnDisconnectToDevice();
        }

        //here come the angles of the plugin
        void OnSendAngles(string stringAngles)
        {
            if (jobstickOnAnglesFromPlugin != null)
            {
                //format %s_%d,%d,%d,%d,%d,%d 
                JobstickAngle angles = JobstickManager.ParseString(stringAngles);
                var addressAndCord = stringAngles.Split('_');
                string address = addressAndCord[0];
                jobstickOnAnglesFromPlugin(address,angles);
            }
        }

        void OnRequestBluetooch(string value)
        {
            if (jobstickOnRequestBluetooch != null)
            {
                jobstickOnRequestBluetooch(value == "true" ? true : false);
            }
                
        }

        void Start ()
        {

        }

        void Update () 
        {
	
        }
    }
}
