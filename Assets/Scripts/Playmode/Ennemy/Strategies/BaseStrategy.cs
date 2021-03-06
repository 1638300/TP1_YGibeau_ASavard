﻿using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public abstract class BaseStrategy : IEnnemyStrategy
    {
        protected readonly Mover Mover;
        protected readonly HandController HandController;
        protected EnnemySensor EnnemySensor;
        protected PickableSensor PickableSensor;
        protected bool IsWorldColliding;

        private WorldSensor frontWorldSensor;
        private WorldSensor backWorldSensor;
        private bool isWorldSeen;

        public BaseStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor frontWorldSensor,
                            WorldSensor backWorldSensor,
                            EnnemySensor ennemySensor, 
                            PickableSensor pickableSensor)
        {
            this.Mover = mover;
            this.frontWorldSensor = frontWorldSensor;
            this.backWorldSensor = backWorldSensor;
            this.EnnemySensor = ennemySensor;
            this.HandController = handController;
            this.PickableSensor = pickableSensor;
            frontWorldSensor.OnWorldSensed += OnWorldSensedFromFront;
            frontWorldSensor.OnWorldUnsensed += OnWorldUnsensedFromFront;
            backWorldSensor.OnWorldSensed += OnWorldSensedFromBack;
            backWorldSensor.OnWorldUnsensed += OnWorldUnsensedFromBack;
            ennemySensor.OnEnnemySensed += OnEnnemySensed;
            ennemySensor.OnEnnemyUnsensed += OnEnnemyUnsensed;
            pickableSensor.OnPickableSensed += OnPickableSensed;
            pickableSensor.OnPickableUnsensed += OnPickableUnsensed;
        }

        public virtual void Act()
        {
            if (isWorldSeen)
            {
                Mover.Rotate(Mover.CLOCKWISE);
            }
            else
            {
                Mover.Move(Mover.Foward);
            }
        }

        private void OnWorldSensedFromFront()
        {
            isWorldSeen = true;
        }

        private void OnWorldUnsensedFromFront()
        {
            isWorldSeen = false;
        }

        private void OnWorldSensedFromBack()
        {
            IsWorldColliding = true;
        }

        private void OnWorldUnsensedFromBack()
        {
            IsWorldColliding = false;
        }

        protected abstract void OnEnnemySensed(EnnemyController ennemy);

        protected abstract void OnEnnemyUnsensed(EnnemyController ennemy);

        protected abstract void OnPickableSensed(PickableController pickable);

        protected abstract void OnPickableUnsensed(PickableController pickable);
    }
}
