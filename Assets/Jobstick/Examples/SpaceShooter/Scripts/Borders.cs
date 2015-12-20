using UnityEngine;

namespace SpaceShooter
{
    public class Borders : MonoBehaviour
    {
        private Collider2D[] colliders;

        public void AddIgnoreCollision(Collider2D value)
        {
            if (colliders == null)
                colliders = this.GetComponentsInChildren<Collider2D>();

            foreach (var collider in colliders)
            {
                Physics2D.IgnoreCollision(value, collider);
            }
        }


        void Start()
        {
        }

        void Update()
        {
        }

    }
}