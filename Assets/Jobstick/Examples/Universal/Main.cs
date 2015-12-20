using JobstickSDK;
using UnityEngine;
using UnityEngine.UI;

namespace Universal
{
    public class Main : MonoBehaviour
    {
        public Text infoLabel;
        public string levelMain;
        public Player player;

        private JobstickUniversal.Mode mode;

        public void OnSwitchToAutoMode()
        {
            JobstickUniversal.mode = JobstickUniversal.Mode.Auto;
            mode = JobstickUniversal.mode;
        }

        public void OnSwitchToInternalMode()
        {
            JobstickUniversal.mode = JobstickUniversal.Mode.Internal;
            mode = JobstickUniversal.mode;
        }

        public void OnSwitchToExternalMode()
        {
            JobstickUniversal.mode = JobstickUniversal.Mode.External;
            mode = JobstickUniversal.mode;
        }

        public void OnComplementaryFilter(bool isOn)
        {
            JobstickUniversal.useComplementaryFilter = isOn;
        }

        public void OnPressButtonBack()
        {
            Application.LoadLevel(levelMain);
        }

        void Awake ()
        {
            OnSwitchToAutoMode();
            Input.gyro.enabled = true;
        }

        void Start()
        {
            if (!Jobstick.IsBluetoothOn())
            {
                Jobstick.RequestBluetooch();
            }
        }

        void OnDestroy()
        {
            Input.gyro.enabled = false;
        }

        string FormatInfoText()
        {
            bool isJobstick = JobstickUniversal.JobstickIsConnect();

            string modeString = "";

            switch (mode)
            {
                case JobstickUniversal.Mode.Internal:
                    modeString = "internal(<color=green>internal</color>)";
                    break;
                case JobstickUniversal.Mode.Auto:
                    modeString = string.Format("auto(<color=green>{0}</color>)", isJobstick ? "jobstick" : "internal");
                    break;
                case JobstickUniversal.Mode.External:
                    modeString = string.Format("external(<color=green>{0}</color>)", isJobstick ? "jobstick" : "no");
                    break;
            }

            if (mode == JobstickUniversal.Mode.External && !isJobstick)
            {
                return "";
            }

            var acelerometrAngle = JobstickUniversal.accelerometrUniversal;
            var gyroscopeAngle = JobstickUniversal.gyroscopeUniversal;

            //Accelerometr values:
            string accelerometrAngleString = string.Format("<color=red>Accelerometr:</color>\n {0,10:f2} {1,10:f2} {2,10:f2}\n\r",
                acelerometrAngle.x, acelerometrAngle.y, acelerometrAngle.z);

            //Gyroscope
            string gyroscopeAngleString = string.Format("<color=teal>Gyroscope:</color>:\n {0,10:f2} {1,10:f2} {2,10:f2}\n\r",
                gyroscopeAngle.x, gyroscopeAngle.y, gyroscopeAngle.z);

            return modeString + "\n" + accelerometrAngleString + "\n" + gyroscopeAngleString + "\n";
        }


        void Update ()
        {
            infoLabel.text = FormatInfoText();
            var acelerometrAngle = JobstickUniversal.accelerometrUniversal;
            player.SetPosition(acelerometrAngle);
        }
    }
}
