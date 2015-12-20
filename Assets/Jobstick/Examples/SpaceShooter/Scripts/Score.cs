using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class Score : MonoBehaviour
    {
        public Text textLeftScore;
        public Text textRightScore;

        private int _leftScore;
        public int leftScore
        {
            set
            {
                _leftScore = value;
                textLeftScore.text = value.ToString();
            }
            get { return _leftScore; }
        }

        private int _rightScore;
        public int righScore
        {
            set
            {
                _rightScore = value;
                textRightScore.text = value.ToString();
            }
            get { return _rightScore; }
        }

        void Start () 
        {
	
        }
        void Update () 
        {
	
        }
    }
}
