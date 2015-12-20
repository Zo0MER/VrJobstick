using UnityEngine;

namespace SpaceShooter
{
    public class IgnoreBorderCollision : MonoBehaviour
    {
        void OnEnable()
        {
            SetIgonore();
        }

        void SetIgonore()
        {
            Collider2D collider = GetComponent<Collider2D>();
            if (collider && Game.instance.borders)
            {
                Game.instance.borders.AddIgnoreCollision(collider);
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
