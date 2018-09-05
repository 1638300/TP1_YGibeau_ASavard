using UnityEngine;

namespace Playmode.Movement
{
    public class AnchoredMover : Mover
    {
        private Transform _rootTransform;

        private new void Awake()
        {
            base.Awake();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _rootTransform = transform.root;
        }

        public override void Move(Vector3 direction)
        {
            transform.Translate(direction.normalized * Speed * Time.deltaTime);
        }

        public override void Rotate(float direction)
        {
            transform.RotateAround(
                _rootTransform.position,
                Vector3.forward,
                (direction < 0 ? RotateSpeed : -RotateSpeed) * Time.deltaTime
            );
        }

        public override void RotateTowards(Vector3 position)
        {
            var directionToTarget = _rootTransform.position - position;
            var angle = Vector3.Angle(_rootTransform.up, directionToTarget);
            var direction = Vector3.Dot(directionToTarget, _rootTransform.right);
            if (angle <= Time.deltaTime* RotateSpeed)
            {
                transform.RotateAround(_rootTransform.position, Vector3.forward, (direction < 0 ? -1 : 1) * (Time.deltaTime * RotateSpeed));
            }
           
        }
    }
}