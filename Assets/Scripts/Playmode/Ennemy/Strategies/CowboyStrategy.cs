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

        public CowboyStrategy(Mover mover, HandController handController, WorldSensor worldSensor, EnnemySensor ennemySensor, PickableSensor pickableSensor)
            : base(mover, handController, worldSensor, ennemySensor, pickableSensor)
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
                Vector3 position = ennemySensor.GetFirstEnnemy.transform.position;
                mover.RotateTowards(position);
                if(Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
                {
                    mover.Move(Mover.Foward);
                }
                handController.Use();
                break;

              case State.PickingWeapon:
                
                break;
            }
        }

        protected override void OnEnnemySensed(EnnemyController ennemy)
        {
            isEnnemySeen = true;
        }

        protected override void OnEnnemyUnsensed(EnnemyController ennemy)
        {
            if(ennemySensor.GetFirstEnnemy == null)
            {
                isEnnemySeen = false;
            }
        }

        protected override void OnPickableSensed(PickableController pickable)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPickableUnsensed(PickableController pickable)
        {
            throw new System.NotImplementedException();
        }

        private enum State
        {
          Seeking,
          Shooting,
          PickingWeapon
        }
    }
}
