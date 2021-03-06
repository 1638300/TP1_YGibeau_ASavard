﻿using Playmode.Ennemy.BodyParts;
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
            if (state == State.Shooting && EnnemySensor.GetFirstEnnemy == null)
            {
                state = State.Seeking;
            }
        }

        protected override void OnPickableSensed(PickableController pickable)
        {
            if (state != State.PickingWeapon && pickable.IsWeapon())
            {
                state = State.PickingWeapon;
                base.Mover.ExtremeSpeedActivated();
            }
        }

        protected override void OnPickableUnsensed(PickableController pickable)
        {
            if (state == State.PickingWeapon && base.PickableSensor.GetFirstWeapon == null)
            {
                if (base.EnnemySensor.GetFirstEnnemy == null)
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
            if (EnnemySensor.GetFirstEnnemy != null)
            {
                Vector3 position = EnnemySensor.GetFirstEnnemy.transform.position;
                Mover.RotateTowards(position);
                if (Vector3.Distance(position, Mover.transform.position) > CLOSEST_DISTANCE_ALLOWED)
                {
                    Mover.Move(Mover.Foward);
                }
                HandController.Use();
            }
        }

        private void MoveTowardsWeapon()
        {
            if (PickableSensor.GetFirstWeapon != null)
            {
                Vector3 position = PickableSensor.GetFirstWeapon.transform.position;
                Mover.RotateTowards(position);
                Mover.Move(Mover.Foward);
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
