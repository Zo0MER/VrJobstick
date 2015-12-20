using Assets.CommonB.UI;
using UnityEngine;

namespace SpaceShooter
{
    public class Meteor : MonoBehaviour
    {
        public int liveCount;
        public Transform newMeteorPrefab;

        private float timerForTheField = 4.0f;

		private FlexibleCamera flexibleCamera;

        void Start ()
        {
			flexibleCamera = Camera.main.GetComponent<FlexibleCamera>();
        }
        void Update ()
        {
            TimerForTheField();
        }

        void TimerForTheField()
        {
			float width = flexibleCamera.width / 2.0f;
			float height = flexibleCamera.height / 2.0f;

            Vector2 position = this.transform.position;

            if (position.x > width || position.x < -width)
            {
                timerForTheField -= Time.deltaTime;
            }

            if (position.y > height || position.y < -height)
            {
                timerForTheField -= Time.deltaTime;
            }

            if (timerForTheField <= 0)
            {
                DestoyThis();
            }
        }


        public void Hit(int damage)
        {
            liveCount -= damage;

            if (liveCount <= 0)
            {
                DestoyThis();
            }
        }

        void DestoyThis()
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);

            if (newMeteorPrefab)
            {
                CreateNewMetiors(Random.Range(4, 6));
            }
        }

        void CreateNewMetiors(int count)
        {
            float offset = 40;
            for (int i = 0; i < count; i++)
            {
                Vector2 position = transform.position;
                Vector2 randPos = new Vector2(Random.Range(position.x - offset, position.x + offset)
                    , Random.Range(position.y - offset, position.y + offset));
                Instantiate(newMeteorPrefab, randPos, this.transform.rotation);
            }
        
        }
    }
}
