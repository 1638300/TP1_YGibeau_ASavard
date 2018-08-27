using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class Normal : BaseStrategy
    {
        private const int closestDistanceAllowed = 3;

        public Normal(Mover mover, HandController handController, WorldSensor worldSensor, EnnemySensor ennemySensor)
            : base(mover, handController, worldSensor, ennemySensor)
        {
            
        }

        public override void Act()
        {
            if (isEnnemySeen)
            {
                Vector3 position = ennemySensor.firstEnnemy.transform.position;
                mover.RotateTowards(position);
                if(Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
                {
                    mover.Move(Mover.Foward);
                }
                handController.Use();
            }
            else
            {
                base.Act();
            }
        }

        protected override void OnEnnemySensed(EnnemyController ennemy)
        {
            isEnnemySeen = true;
        }

        protected override void OnEnnemyUnsensed(EnnemyController ennemy)
        {
            if(ennemySensor.firstEnnemy == null)
            {
                isEnnemySeen = false;
            }
        }
    }
}
