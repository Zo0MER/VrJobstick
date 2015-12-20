using System;
using System.Collections;
using JobstickSDK;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Table
{
    public class DeviceInfoItem : MonoBehaviour
    {
        public string deviceAddress = "Test";
        public Action<DeviceInfoItem> OnPressButtonDisconnect;
        public Text infoText;

        public string text
        {
            set { infoText.text = value; }
        }

        public void OnDisconnect()
        {
            Debug.Log("OnDisconnect");
            if (OnPressButtonDisconnect != null)
            {
                OnPressButtonDisconnect(this);
            }
        }

        void Start ()
        {
            //StartCoroutine(Demo());
        }

        IEnumerator Demo()
        {
            while (true)
            {
                JobstickAngle angles = new JobstickAngle();

                angles.ax = Rand();
                angles.ay = Rand();
                angles.az = Rand();
                angles.gx = Rand();
                angles.gy = Rand();
                angles.gz = Rand();

                SetAngleValue(angles);
                
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        
        void Update ()
        {
            JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(deviceAddress);
            SetAngleValue(angle);
        }

        void SetAngleValue(JobstickAngle angle)
        {
            if (angle != null)
            {
                string address = string.Format("Device: <color=green><b>{0}</b></color>\n\r", deviceAddress);

                //Raw values:
                string rawLable = "<color=red>Raw values</color>:\n";
                string info = "   ax     ay     az     ga     gx     gz\n\r";
                string rawValues = string.Format(" {0,6} {1,6} {2,6} {3,6} {4,6} {5,6}\n\r",
                    angle.ax, angle.ay, angle.az, angle.gx, angle.gy, angle.gz);

                //Vector3 raw
                float angleRawX = angle.ay / 200f;
                float angleRawY = angle.ax / -200f;
                float angleRawZ = angle.az / 200f;
                string angleRawString = string.Format("<color=teal>Angle raw</color>:\n {0,10} {1,10} {2,10}\n\r",
                    angleRawX, angleRawY, angleRawZ);

                //Angle fix
                float angleFixX = angle.fixedX;
                float angleFixY = angle.fixedY;
                float angleFixZ = angle.fixedZ;
                string angleFixString = string.Format("<color=magenta>Angle fix</color>:\n {0,10} {1,10} {2,10}",
                    angleFixX, angleFixY, angleFixZ);

                text = address + rawLable + info + rawValues + angleRawString + angleFixString;
            }
        }

        int Rand()
        {
            return Random.Range(-65000, 65000);
        }
    }
}
