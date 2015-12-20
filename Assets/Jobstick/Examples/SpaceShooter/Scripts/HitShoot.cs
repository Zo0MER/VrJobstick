using System.Collections;
using Assets.CommonB;
using UnityEngine;

namespace SpaceShooter
{
    public class HitShoot : MonoBehaviour
    {
        public float timeRemove = 0.2f;
        private SpriteRenderer sprite;

        void Start ()
        {
            sprite = GetComponent<SpriteRenderer>();
            StartCoroutine(FlashHit());
        }
        void Update () 
        {
	
        }

        IEnumerator FlashHit()
        {
            sprite.color = new Color(sprite.color.r , sprite.color.g , sprite.color.b , 0.0f);

            yield return StartCoroutine(ActionGenerator.SmoothChangeAlpha(sprite, 1.0f, 0.1f));
            yield return StartCoroutine(ActionGenerator.SmoothChangeAlpha(sprite, 0.4f, 0.1f));

            this.DestroyObject();
        }
    }
}
