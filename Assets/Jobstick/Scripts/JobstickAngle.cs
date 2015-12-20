using System;

namespace JobstickSDK
{
    public class JobstickAngle
    {
        private static double M_PI = 3.14159265358979323846;
        private static double RADIANS_TO_DEGREES = 180.0 / M_PI;
        private static double KOEFF = 0.991;
        private static double DT = 15.0 / 1000.0;

        public int ax;
        public int ay;
        public int az;

        public int gx;
        public int gy;
        public int gz;

        public float fixedX = 0;
        public float fixedY = 0;
        public float fixedZ = 0;

        private long timeStamp;

        public JobstickAngle()
        {
            timeStamp = System.DateTime.Now.Ticks;
        }

        public void SetLastAngle(JobstickAngle lastAngle)
        {
            if (lastAngle == null)
            {
                fixedX = 0;
                fixedY = 0;
                fixedZ = 0;
            }
            else
            {
                fixedX = lastAngle.fixedX;
                fixedY = lastAngle.fixedY;
                fixedZ = lastAngle.fixedZ;
            }

            double gyro_x = (float)gx / 131.0;
            double gyro_y = (float)gy / 131.0;
            double gyro_z = (float)gz / 131.0;

            double accel_angle_y = Math.Atan(-1 * ax / Math.Sqrt(ay * ay + az * az)) * RADIANS_TO_DEGREES;
            double accel_angle_x = Math.Atan(ay / Math.Sqrt(ax * ax + az * az)) * RADIANS_TO_DEGREES;

            double gyro_angle_x = gyro_x * DT + fixedX;
            double gyro_angle_y = gyro_y * DT + fixedY;
            double gyro_angle_z = gyro_z * DT + fixedZ;

            fixedX = (float)(KOEFF * gyro_angle_x + (1.0 - KOEFF) * accel_angle_x);
            fixedY = (float)(KOEFF * gyro_angle_y + (1.0 - KOEFF) * accel_angle_y);
            fixedZ = (float)(gyro_angle_z);
        }

        public new string ToString()
        {
            return string.Format("{0} {1} {2}", ax, ay, az);
        }
    }
}