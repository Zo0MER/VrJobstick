using System.Collections;
using System.Collections.Generic;
using Assets.CommonB.UI;
using UnityEngine;

namespace SpaceShooter
{
    public class MeteorGenerator : MonoBehaviour
    {
        public Transform bigMeteorPrefab;
        public Transform smallMeteorPrefab;

		public FlexibleCamera flexibleCamera;

        public int maxBigMeteorsToScene = 2;
        public int maxSmallMeteorsToScene = 15;

        private List<Transform> bigMeteors = new List<Transform>();
        private List<Transform> smallMeteors = new List<Transform>();

        private const float offsetToBorders = 50.0f;

        void Start ()
        {
            
        }

		public void StartGenerator()
		{
			StopGenerator ();
			StartCoroutine(Generator());
		}

		public void StopGenerator()
		{
			StopAllCoroutines ();
		}

        IEnumerator Generator()
        {
            while (true)
            {
                bigMeteors.Remove(null);
                smallMeteors.Remove(null);

                if (bigMeteors.Count < maxBigMeteorsToScene)
                {
                    CreateBigMeteor();
                }

                if (smallMeteors.Count < maxSmallMeteorsToScene)
                {
                    CreateSmallMeteor();
                }

                yield return new WaitForSeconds(1.2f);
            }
        }

        void CreateBigMeteor()
        {
            Vector2 pointToMove = new Vector2();
            var meteor = CreateMeteor(bigMeteorPrefab);
            bigMeteors.Add(meteor);
            AddForce(meteor, pointToMove, 100000.0f, 500000.0f);
        }

        void CreateSmallMeteor()
        {
            Vector2 pointToMove = new Vector2();
            var meteor = CreateMeteor(smallMeteorPrefab);
            smallMeteors.Add(meteor);
            AddForce(meteor, pointToMove, 1000.0f, 5000.0f);
        }

        private Transform CreateMeteor(Transform prefab)
        {
            Transform meteor = Instantiate(prefab, RandomPosition(), transform.rotation) as Transform;
            meteor.parent = transform;
            return meteor;
        }

        private void AddForce(Transform meteor, Vector2 pointToMove , float min , float  max)
        {
            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            float speed = Random.Range(min, max);
            Vector2 direction = rb.position - pointToMove;
            rb.AddForceAtPosition(-direction.normalized*speed, pointToMove);
        }

        Vector2 RandomPosition()
        {
			float width = flexibleCamera.width / 2.0f;
			float height = flexibleCamera.height / 2.0f;

            float x = 0;
            float y = 0;

            if (Random.Range(0, 2) == 1)
            {
                if (Random.Range(0, 2) == 1) x = Random.Range(width, width + offsetToBorders);
                else x = Random.Range(-width, -width - offsetToBorders);

                y = Random.Range(-height, height);
            }
            else
            {
                if (Random.Range(0, 2) == 1) y = Random.Range(height, height + offsetToBorders);
                else y = Random.Range(-height, -height - offsetToBorders);

                x = Random.Range(-width, width);
            }
            return new Vector2(x, y);
        }

		public void RemoveAllMeteors()
		{
			RemoveMeteorsInList (smallMeteors);
			RemoveMeteorsInList (bigMeteors);
		}

		void RemoveMeteorsInList(List<Transform>  list)
		{
			foreach(var value in list)
			{
				value.gameObject.SetActive(false);
				Destroy(value.gameObject);
			}
		}


        void Update () 
        {
	
        }
    }
}
