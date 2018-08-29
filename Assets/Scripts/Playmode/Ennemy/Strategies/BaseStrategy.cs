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
        protected bool isEnnemySeen;
        protected EnnemySensor ennemySensor;
        protected PickableSensor pickableSensor;

        private WorldSensor worldSensor;
        private bool isWorldSeen;

        public BaseStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor worldSensor, 
                            EnnemySensor ennemySensor, 
                            PickableSensor pickableSensor)
        {
            this.mover = mover;
            this.worldSensor = worldSensor;
            this.ennemySensor = ennemySensor;
            this.handController = handController;
            this.pickableSensor = pickableSensor;
            worldSensor.OnWorldSensed += OnWorldSensed;
            worldSensor.OnWorldUnsensed += OnWorldUnsensed;
            ennemySensor.OnEnnemySensed += OnEnnemySensed;
            ennemySensor.OnEnnemyUnsensed += OnEnnemyUnsensed;
            pickableSensor.OnPickableSensed += OnPickableSensed;
            pickableSensor.OnPickableUnsensed += OnPickableUnsensed;
        }

        ~BaseStrategy()
        {
            worldSensor.OnWorldSensed -= OnWorldSensed;
            worldSensor.OnWorldUnsensed -= OnWorldUnsensed;
            ennemySensor.OnEnnemySensed -= OnEnnemySensed;
            ennemySensor.OnEnnemyUnsensed -= OnEnnemyUnsensed;
            pickableSensor.OnPickableSensed -= OnPickableSensed;
            pickableSensor.OnPickableUnsensed -= OnPickableUnsensed;
        }

        public virtual void Act()
        {
            if (isWorldSeen)
            {
                mover.Rotate(Mover.Clockwise);
            }
            else
            {
                mover.Move(Mover.Foward);
            }
        }

        private void OnWorldSensed()
        {
            isWorldSeen = true;
        }

        private void OnWorldUnsensed()
        {
            isWorldSeen = false;
        }

        protected abstract void OnEnnemySensed(EnnemyController ennemy);

        protected abstract void OnEnnemyUnsensed(EnnemyController ennemy);

        protected abstract void OnPickableSensed(PickableController pickable);

        protected abstract void OnPickableUnsensed(PickableController pickable);
    }
}
