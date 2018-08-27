using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public abstract class BaseStrategy : IEnnemyStrategy
    {
        protected readonly Mover mover;
        protected readonly HandController handController;
        protected bool isEnnemySeen;
        protected EnnemySensor ennemySensor;

        private WorldSensor worldSensor;
        private bool isWorldSeen;

        public BaseStrategy(Mover mover, HandController handController, WorldSensor worldSensor, EnnemySensor ennemySensor)
        {
            this.mover = mover;
            this.worldSensor = worldSensor;
            this.ennemySensor = ennemySensor;
            this.handController = handController;
            worldSensor.OnWorldSensed += OnWorldSensed;
            worldSensor.OnWorldUnsensed += OnWorldUnsensed;
            ennemySensor.OnEnnemySensed += OnEnnemySensed;
            ennemySensor.OnEnnemyUnsensed += OnEnnemyUnsensed;
        }

        ~BaseStrategy()
        {
            worldSensor.OnWorldSensed -= OnWorldSensed;
            worldSensor.OnWorldUnsensed -= OnWorldUnsensed;
            ennemySensor.OnEnnemySensed -= OnEnnemySensed;
            ennemySensor.OnEnnemyUnsensed -= OnEnnemyUnsensed;
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
    }
}
