using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Table
{
    public class ScaningLabel : MonoBehaviour
    {
        private string defaultText = "Scanning";
        private Text label;

        void OnEnable()
        {
            StartCoroutine(Scanning());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator Scanning()
        {
            int count = 4;

            while (true)
            {
                if (count > 3)
                {
                    label.text = defaultText;
                    count = 0;
                }

                yield return new WaitForSeconds(1.0f);

                label.text += ".";
                count++;
            }
        }

        void Awake()
        {
            label = GetComponent<Text>();
        }

        void Start ()
        {
	    
        }
        void Update () 
        {
	
        }
    }
}
