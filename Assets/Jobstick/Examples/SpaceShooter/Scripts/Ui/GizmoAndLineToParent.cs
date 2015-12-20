using UnityEngine;

namespace SpaceShooter.Ui
{
    public class GizmoAndLineToParent : MonoBehaviour
    {
        public float _radius = 2.0f;
        public Transform _drawLineFrom;
        // Use this for initialization
        void Start () 
        {
	
        }

        void Update()
        {
        
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, _radius);

            Transform draw = _drawLineFrom ? _drawLineFrom : transform.parent;

            if (draw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, draw.position);
            }
        }
    }
}
