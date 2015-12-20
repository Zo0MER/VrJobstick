using UnityEngine;

namespace Universal
{
    public class Player : MonoBehaviour
    {
        public float maxX;
        public float maxY;

        public RectTransform fieldRect;
        private RectTransform playerRect;

        // public Vector3 pos;

        public void SetPosition(Vector2 position)
        {
            float fieldW = fieldRect.rect.width;
            float fieldH = fieldRect.rect.height;

            float xFix = (fieldW * 0.5f) / maxX;
            float yFix = (fieldH * 0.5f) / maxY;

            float posX = xFix * Mathf.Clamp(position.x, -maxX, maxX);
            float posY = yFix * Mathf.Clamp(position.y, -maxY, maxY);

			playerRect.anchoredPosition = new Vector3(posX, posY , 0);
        }

        void Start()
        {
            playerRect = GetComponent<RectTransform>();
        }
        void Update()
        {
            //SetPosition(pos);
        }
    }
} 

