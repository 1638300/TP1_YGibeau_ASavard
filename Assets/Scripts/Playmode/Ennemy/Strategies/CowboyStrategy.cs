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
        private const int closestDistanceAllowed = 3;
        private State state = State.Seeking;

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
            switch (state)
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
            if (state == State.Seeking)
            {
                state = State.Shooting;
            }
        }

        protected override void OnEnnemyUnsensed(EnnemyController ennemy)
        {
            if (state == State.Shooting && ennemySensor.GetFirstEnnemy == null)
            {
                state = State.Seeking;
            }
        }

        protected override void OnPickableSensed(PickableController pickable)
        {
            if (state != State.PickingWeapon && pickable.IsWeapon())
            {
                state = State.PickingWeapon;
                base.mover.ExtremeSpeedActivated();
            }
        }

        protected override void OnPickableUnsensed(PickableController pickable)
        {
            if (state == State.PickingWeapon && base.pickableSensor.GetFirstWeapon() == null)
            {
                if (base.ennemySensor.GetFirstEnnemy == null)
                {
                    state = State.Seeking;
                }
                else
                {
                    state = State.Shooting;
                }
            }
        }

        private void MoveAndShootTowardsEnnemy()
        {
            if (ennemySensor.GetFirstEnnemy != null)
            {
                Vector3 position = ennemySensor.GetFirstEnnemy.transform.position;
                mover.RotateTowards(position);
                if (Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
                {
                    mover.Move(Mover.Foward);
                }
                handController.Use();
            }
        }

        //Todo : try/catch?
        private void MoveTowardsWeapon()
        {
            if (pickableSensor.GetFirstWeapon() != null)
            {
                Vector3 position = pickableSensor.GetFirstWeapon().transform.position;
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
