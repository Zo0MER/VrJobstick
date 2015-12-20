using Assets.CommonB.UI;
using UnityEngine;

namespace SpaceShooter
{
    public class BackgroundStars : MonoBehaviour
    {
		public FlexibleCamera flexibleCamera;
        public Transform starBig;
        public Transform starSmall;

        public int countStars = 50;
        public float probabilityBigStar = 0.2f;

        void Start()
        {
            for (int i = 0; i < countStars; i++)
            {
				float width = flexibleCamera.width / 2.0f;
				float height = flexibleCamera.height / 2.0f;

                Transform star;

                Vector3 starPosition = new Vector2(Random.Range(-width, width), Random.Range(-height, height)); ;

                starPosition += new Vector3(0, 0, 30);

                if (Random.Range(0, 1.0f) < probabilityBigStar)
                {
                    star = Instantiate(starBig, starPosition, Quaternion.identity) as Transform;
                }
                else
                {
                    star = Instantiate(starSmall, starPosition, Quaternion.identity) as Transform;
                }

                star.parent = this.transform;
            }
        }
        void Update()
        {

        }
    } 
}

