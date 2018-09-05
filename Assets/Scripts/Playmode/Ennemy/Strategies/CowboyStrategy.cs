using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using Playmode.Pickables.Types;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CowboyStrategy : BaseStrategy
    {
        private const int CLOSEST_DISTANCE_ALLOWED = 3;
        private State _state = State.Seeking;

        public CowboyStrategy(
                            Mover mover,
                            HandController handController,
                            WorldSensor frontWorldSensor,
                            WorldSensor backWorldSensor,
                            EnnemySensor ennemySensor,
                            PickableSensor pickableSensor)
            : base(mover, handController, frontWorldSensor, backWorldSensor, ennemySensor, pickableSensor)
        {

        }

        public override void Act()
        {
            switch (_state)
            {
                case State.Seeking:
                    base.Act();
                    break;

                case State.Shooting:
                    MoveAndShootTowardsEnnemy();
                    break;

                case State.PickingWeapon:
                    MoveTowardsWeapon();
                    break;
            }
        }

        protected override void OnEnnemySensed(EnnemyController ennemy)
        {
            if (_state == State.Seeking)
            {
                _state = State.Shooting;
            }
        }

        protected override void OnEnnemyUnsensed(EnnemyController ennemy)
        {
            if (_state == State.Shooting && ennemySensor.GetFirstEnnemy == null)
            {
                _state = State.Seeking;
            }
        }

        protected override void OnPickableSensed(PickableController pickable)
        {
            if (_state != State.PickingWeapon && pickable.IsWeapon())
            {
                _state = State.PickingWeapon;
                base.mover.ExtremeSpeedActivated();
            }
        }

        protected override void OnPickableUnsensed(PickableController pickable)
        {
            if (_state == State.PickingWeapon && base.pickableSensor.GetFirstWeapon == null)
            {
                if (base.ennemySensor.GetFirstEnnemy == null)
                {
                    _state = State.Seeking;
                }
                else
                {
                    _state = State.Shooting;
                }
            }
        }

        private void MoveAndShootTowardsEnnemy()
        {
            if (ennemySensor.GetFirstEnnemy != null)
            {
                Vector3 position = ennemySensor.GetFirstEnnemy.transform.position;
                mover.RotateTowards(position);
                if (Vector3.Distance(position, mover.transform.position) > CLOSEST_DISTANCE_ALLOWED)
                {
                    mover.Move(Mover.Foward);
                }
                handController.Use();
            }
        }

        private void MoveTowardsWeapon()
        {
            if (pickableSensor.GetFirstWeapon != null)
            {
                Vector3 position = pickableSensor.GetFirstWeapon.transform.position;
                mover.RotateTowards(position);
                mover.Move(Mover.Foward);
            }
        }

        private enum State
        {
            Seeking,
            Shooting,
            PickingWeapon
        }
    }
}
