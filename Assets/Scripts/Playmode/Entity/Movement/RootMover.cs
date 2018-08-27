using UnityEngine;

namespace Playmode.Movement
{
    public class RootMover : Mover
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
            rootTransform.Translate(direction.normalized * speed * Time.deltaTime);
        }

        public override void Rotate(float direction)
        {
            rootTransform.Rotate(
                Vector3.forward,
                (direction < 0 ? rotateSpeed : -rotateSpeed) * Time.deltaTime
            );
        }

        public override void RotateTowards(Vector3 position)
        {
            var directionToTarget = rootTransform.position - position;
            var angle = Vector3.Angle(rootTransform.up, directionToTarget);
            var direction = Vector3.Dot(directionToTarget, rootTransform.right);
            rootTransform.Rotate(Vector3.forward, (direction < 0 ? -1 : 1) * Mathf.Min(angle, rotateSpeed * Time.deltaTime));
        }
    }
}