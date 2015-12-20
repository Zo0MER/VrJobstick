using JobstickSDK;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : MonoBehaviour
    {
        public KeybordControl keybordControl;
        public float maxAngularVelocity = 500;
        public string addressJobstick;

        public TextMesh textInfo;

        private float correctionXY = 0.5f;

        void FixedUpdate()
        {
            KeybordInput();
            JobstickInput();
        }

        private void JobstickInput()
        {
            JobstickAngle angle = Jobstick.controller.GetAnglesToPlayerFromAddress(addressJobstick);
            if (angle != null)
            {
				string info = string.Format("{0}\nax = {1}\nay = {2}\naz = {3}", addressJobstick  , angle.ax, angle.ay, angle.az);

				if(Game.instance.isShowDebugInfo)
					textInfo.text = info;
				else textInfo.text = "";

				//
				
				GetComponent<Rigidbody2D>().AddForce(-Vector2.up * angle.ax * correctionXY);
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * angle.ay * correctionXY);
				//
				float force = -angle.gz / 200.0f;
				GetComponent<Rigidbody2D>().angularVelocity = force;
            }
        }

        private void KeybordInput()
        {
            float force = 0;

            if (keybordControl.keyLeftPress)
            {
                force = GetComponent<Rigidbody2D>().angularVelocity + maxAngularVelocity * Time.fixedDeltaTime;
            }
            else if (keybordControl.keyRightPress)
            {
                force = GetComponent<Rigidbody2D>().angularVelocity + -maxAngularVelocity * Time.fixedDeltaTime;
            }
            GetComponent<Rigidbody2D>().angularVelocity = force;


            Vector2 addForce = new Vector2();

            if (keybordControl.keyUpPress)
            {
                addForce = Vector2.up * 1500;
            }
            else if (keybordControl.keyDownPress)
            {
                addForce = Vector2.up * -1500;
            }

            GetComponent<Rigidbody2D>().AddRelativeForce(addForce);
        }

        void Start()
        {

        }
        void Update()
        {

        }
    }
}
