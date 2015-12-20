using UnityEngine;
using System.Collections;

namespace SpaceShooter
{
    public class ConfigPanel : MonoBehaviour
    {
        public Transform infoPanel;

        public bool _isShowInfo;
        public bool isShowInfo
        {
            set
            {
                _isShowInfo = value;
                infoPanel.gameObject.SetActive(value);
            }
            get { return _isShowInfo; }
        }

        public void PressButtonConfig()
        {
            bool isActive = this.gameObject.activeSelf;
            this.gameObject.SetActive(!isActive);

            Time.timeScale = !isActive ? 0.0f : 1.0f;
        }

        void Start()
        {
            this.gameObject.SetActive(false);
        }
        void Update()
        {

        }
    }  
}

