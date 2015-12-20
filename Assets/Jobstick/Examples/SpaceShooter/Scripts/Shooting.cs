using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    public class Shooting : MonoBehaviour
    {
        public Transform bulletPrefab;
        public Transform gunPoint;

        public int damageShot;
        public float delayShots = 0.5f;

        public void Shot()
        {
            CreateBullet();
        }

        void CreateBullet()
        {
            Transform bulletObject = Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.rotation) as Transform;
            bulletObject.parent = this.transform;

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.OnHit = SendHitMessage;
            bullet.damageCount = damageShot;
        }

        void SendHitMessage(GameObject obj)
        {
            if (obj.tag == "Big")
            {
                Game.instance.score.leftScore += 10;
            }
            else if (obj.tag == "Small")
            {
                Game.instance.score.leftScore += 5;
            }
        }

        IEnumerator ShotDelay()
        {
            while (true)
            {
                Shot();
                yield return new WaitForSeconds(delayShots);
            }
        }

        void Start()
        {
            StartCoroutine(ShotDelay());
        }
        void Update()
        {

        }
    }
}


