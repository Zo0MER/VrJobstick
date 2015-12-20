using UnityEngine;
using UnityEngine.UI;

namespace Table
{
    public class ButtonStartScan : MonoBehaviour
    {
        public Text textLabel;
        public string textStartScan;
        public string textStopScan;

        public void SetStartScan()
        {
            textLabel.text = textStartScan;
        }

        public void SetStopScan()
        {
            textLabel.text = textStopScan;
        }

        void Start () 
        {
	
        }
        void Update () 
        {
	
        }
    }
}
