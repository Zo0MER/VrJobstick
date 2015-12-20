using UnityEngine;

namespace JobstickSDK
{
    public class JobstickAndroid 
#if UNITY_ANDROID
        : IJobstickNative
#endif
    {
#if UNITY_ANDROID
        public AndroidJavaObject jObject = null;

        public JobstickAndroid ()
        {
            Init();
        }

        public void Init()
        {
            if (!Application.isEditor)
            {
                using (var unityPlayer = new AndroidJavaClass("com.shda.JobstickPlugin.StartActivity"))
                {
                    jObject = unityPlayer.CallStatic<AndroidJavaObject>("instance");
                }
            }
        }

        public void StartSearchingDevices()
        {
            CallJavaMethod("StartScan");
        }

        public void StopSearchingDevices()
        {
            CallJavaMethod("StopScan");
        }

        public void DisconnectAllPlayers()
        {
            CallJavaMethod("DisconnectAllPlayers");
        }

        public void DisconnectPlayerAtAddress(string address)
        {
            CallJavaMethod("CloseConnectionPlayer" , address);
        }

        public void SetMaxPlayers(int max)
        {
            if (jObject != null)
            {
                jObject.Call("SetMaxPlayers", max);
            }
        }

        public void BluetoothOn()
        {
            CallJavaMethod("BluetoothEnable");
        }

        public void BluetoothOff()
        {
            CallJavaMethod("BluetoothDisable");
        }

        public bool IsBluetoothOn()
        {
            if (jObject != null)
            {
                return jObject.Call<bool>("IsBluetoothOn");
            }

            return false;
        }

        public JobstickAngle GetAngles()
        {
            if (jObject != null)
            {
                string anglesString = jObject.Get<string>("anglesString");

                return JobstickManager.ParseString(anglesString);
            }

            return null;
        }

        public void RequestBluetooch()
        {
            CallJavaMethod("RequestBluetooch");
        }

        void CallJavaMethod(string name, string value = null)
        {
            if (jObject != null)
            {
                Debug.Log("Native + " + name);

                if (value == null)
                {
                    jObject.Call(name);
                }
                else
                {
                    jObject.Call(name, value);
                }
            }
        }
#endif
    }
}
