using UnityEngine;

namespace SpaceShooter
{
    public class KeybordControl : MonoBehaviour
    {
        public bool keyLeftPress;
        public bool keyRightPress;

        public bool keyUpPress;
        public bool keyDownPress;

        void Start () 
        {
	    
        }

        void Update ()
        {
            KeybordUpdate();
        }

        void KeybordUpdate()
        {
            KebordKeyDown();
        }

        private void KebordKeyDown()
        {
            keyLeftPress = false;
            keyRightPress = false;
            keyUpPress = false;
            keyDownPress = false;

            if (IsKeyPress(KeyCode.LeftArrow))
            {
                keyLeftPress = true;
            }

            if (IsKeyPress(KeyCode.RightArrow))
            {
                keyRightPress = true;
            }

            if (IsKeyPress(KeyCode.UpArrow))
            {
                keyUpPress = true;
            }

            if (IsKeyPress(KeyCode.DownArrow))
            {
                keyDownPress = true;
            }
        }

        bool IsKeyPress(KeyCode key)
        {
            return Input.GetKey(key);
        }
    }
}

