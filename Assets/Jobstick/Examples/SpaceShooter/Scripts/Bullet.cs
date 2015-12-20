using System;
using System.Collections;
using Assets.CommonB;
using UnityEngine;

namespace SpaceShooter
{
    public class Bullet : MonoBehaviour
    {
        public Transform hitSprite;
        public float timeLive = 1.0f;
        public float damageCount = 1.0f;
        public float speed;

        public Action<GameObject> OnHit; 

        void Start () 
        {
            Vector2 addForce = Vector2.up * speed;
            GetComponent<Rigidbody2D>().AddRelativeForce(addForce);

            StartCoroutine(TimeLive());
        }
        void Update () 
        {
	
        }

        IEnumerator TimeLive()
        {
            yield return new WaitForSeconds(timeLive);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision2D)
        {
            ContactPoint2D[] points = collision2D.contacts;
            Instantiate(hitSprite, new Vector3(points[0].point.x, points[0].point.y) + new Vector3(0, 0, -10.0f), this.transform.rotation);

            this.DestroyObject();

            SendHitMessage(collision2D.gameObject);
        }

        public void SendHitMessage(GameObject obj)
        {
            if (obj){
                obj.SendMessage("Hit", damageCount, SendMessageOptions.DontRequireReceiver);
            }

            if (OnHit != null){
                OnHit(obj);
            }
        }
    }
}
