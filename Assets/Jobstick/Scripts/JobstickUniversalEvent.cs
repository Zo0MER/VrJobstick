using System;
using UnityEngine;
using System.Collections;

namespace JobstickSDK
{
    public class JobstickUniversalEvent : MonoBehaviour
    {
        public Vector3 gyroscopeAngle { get; private set; }
        public Vector3 accelerometrAngle { get; private set; }

        public event Action<JobstickUniversal.SensorType, Vector3> OnSensorUpdate;

        public bool useComplementaryFilter { get; set; }

        private const float MAGIC_KOEFF_RAW = 17000f;
        private const float MAGIC_KOEFF_FIXED = 90f;
        private const float MAGIC_KOEFF_GYRO = -9800f;

        private JobstickUniversal.Mode _mode;

        public JobstickUniversal.Mode mode
        {
            set
            {
                SwitchMode(value);
                _mode = value;
            }
            get { return _mode; }
        }

        public void SwitchMode(JobstickUniversal.Mode toMode)
        {
            if (toMode != mode)
            {
                switch (toMode)
                {
                    case JobstickUniversal.Mode.Internal:
                        Jobstick.controller.DisconnectAllPlayers();
                        Jobstick.controller.StopSearchDevices();
                        break;
                    case JobstickUniversal.Mode.Auto:
                    case JobstickUniversal.Mode.External:
                        Jobstick.controller.StartSearchDevices();
                        break;

                }
            }
        }

        public JobstickUniversalEvent()
        {
            gyroscopeAngle = new Vector3();
            accelerometrAngle = new Vector3();
        }

        private Vector3 AngleFromGyroscope()
        {
            Vector3 angle = new Vector3();
            angle.x = Input.gyro.rotationRate.x;
            angle.y = Input.gyro.rotationRate.y;
            angle.z = Input.gyro.rotationRate.z;

            return angle;
        }

        private Vector3 AngleFromAccelerometr()
        {
            Vector3 angle = new Vector3();
            angle.x = Input.acceleration.x;
            angle.y = Input.acceleration.y;
            angle.z = Input.acceleration.z;

            return angle;
        }

        void Start()
        {
            useComplementaryFilter = false;
        }

        void Update()
        {
            UpdateSensor();
        }

        void UpdateSensor()
        {
            if (_mode == JobstickUniversal.Mode.Internal)
            {
                SendAglesFromInternal();
            }
            else if (_mode == JobstickUniversal.Mode.External)
            {
                if (Jobstick.controller.GetCountPlayers() > 0)
                {
                    SendAngleFromJobstick();
                }
            }
            else if (_mode == JobstickUniversal.Mode.Auto)
            {
                if (Jobstick.controller.GetCountPlayers() > 0)
                {
                    SendAngleFromJobstick();
                }
                else
                {
                    SendAglesFromInternal();
                }
            }
        }

        void SetAngle(JobstickUniversal.SensorType type , Vector3 angle)
        {
            if (type == JobstickUniversal.SensorType.Gyroscope)
            {
                gyroscopeAngle = angle;
            }
            else if (type == JobstickUniversal.SensorType.Accelerometer)
            {
                accelerometrAngle = angle;
            }

            if (OnSensorUpdate != null)
            {
                OnSensorUpdate(type , angle);
            }
        }

        void SendAglesFromInternal()
        {
            SetAngle(JobstickUniversal.SensorType.Accelerometer, AngleFromAccelerometr());
            SetAngle(JobstickUniversal.SensorType.Gyroscope, AngleFromGyroscope());
        }

        void SendAngleFromJobstick()
        {
            var angle = Jobstick.controller.GetAnglesFirstPlayer();
            if (angle != null)
            {
                if (useComplementaryFilter)
                {
                    float x = (float)(angle.fixedX / -MAGIC_KOEFF_FIXED);
                    float y = (float)(angle.fixedY / -MAGIC_KOEFF_FIXED);
                    float z = angle.az / MAGIC_KOEFF_RAW * (-1);

                    SetAngle(JobstickUniversal.SensorType.Accelerometer, new Vector3(x, y, z));
                    SetAngle(JobstickUniversal.SensorType.Gyroscope, new Vector3(angle.gx / MAGIC_KOEFF_GYRO, angle.gy / MAGIC_KOEFF_GYRO, angle.gz / MAGIC_KOEFF_GYRO));
                }
                else
                {
                    float x = angle.ax / MAGIC_KOEFF_RAW * (-1);
                    float y = angle.ay / MAGIC_KOEFF_RAW * (-1);
                    float z = angle.az / MAGIC_KOEFF_RAW * (-1);

                    SetAngle(JobstickUniversal.SensorType.Accelerometer, new Vector3(x, y, z));
                    SetAngle(JobstickUniversal.SensorType.Gyroscope, new Vector3(angle.gx / MAGIC_KOEFF_GYRO, angle.gy / MAGIC_KOEFF_GYRO, angle.gz / MAGIC_KOEFF_GYRO));
                }
            }
        }
    }
}

