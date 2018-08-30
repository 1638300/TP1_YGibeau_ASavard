using UnityEngine;

namespace Playmode.Movement
{
    public class AnchoredMover : Mover
    {
        private Transform rootTransform;

        private new void Awake()
        {
            base.Awake();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            rootTransform = transform.root;
        }

        public override void Move(Vector3 direction)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }

        public override void Rotate(float direction)
        {
            transform.RotateAround(
                rootTransform.position,
                Vector3.forward,
                (direction < 0 ? rotateSpeed : -rotateSpeed) * Time.deltaTime
            );
        }

        public override void RotateTowards(Vector3 position)
        {
            var directionToTarget = rootTransform.position - position;
            var angle = Vector3.Angle(rootTransform.up, directionToTarget);
            var direction = Vector3.Dot(directionToTarget, rootTransform.right);
            if (angle <= Time.deltaTime* rotateSpeed)
            {
                transform.RotateAround(rootTransform.position, Vector3.forward, (direction < 0 ? -1 : 1) * (Time.deltaTime * rotateSpeed));
            }
           
        }
    }
}