using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public abstract class BaseStrategy : IEnnemyStrategy
    {
        protected readonly Mover mover;
        protected readonly HandController handController;
        protected EnnemySensor ennemySensor;
        protected PickableSensor pickableSensor;
        protected bool isWorldColliding;

        private WorldSensor _frontWorldSensor;
        private WorldSensor _backWorldSensor;
        private bool _isWorldSeen;

        public BaseStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor frontWorldSensor,
                            WorldSensor backWorldSensor,
                            EnnemySensor ennemySensor, 
                            PickableSensor pickableSensor)
        {
            this.mover = mover;
            this._frontWorldSensor = frontWorldSensor;
            this._backWorldSensor = backWorldSensor;
            this.ennemySensor = ennemySensor;
            this.handController = handController;
            this.pickableSensor = pickableSensor;
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
            if (_isWorldSeen)
            {
                mover.Rotate(Mover.CLOCKWISE);
            }
            else
            {
                mover.Move(Mover.Foward);
            }
        }

        private void OnWorldSensedFromFront()
        {
            _isWorldSeen = true;
        }

        private void OnWorldUnsensedFromFront()
        {
            _isWorldSeen = false;
        }

        private void OnWorldSensedFromBack()
        {
            isWorldColliding = true;
        }

        private void OnWorldUnsensedFromBack()
        {
            isWorldColliding = false;
        }

        protected abstract void OnEnnemySensed(EnnemyController ennemy);

        protected abstract void OnEnnemyUnsensed(EnnemyController ennemy);

        protected abstract void OnPickableSensed(PickableController pickable);

        protected abstract void OnPickableUnsensed(PickableController pickable);
    }
}
