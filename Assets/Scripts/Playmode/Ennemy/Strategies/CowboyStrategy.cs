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
                Vector3 position = ennemySensor.firstEnnemy.transform.position;
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
            if(ennemySensor.firstEnnemy == null)
            {
                isEnnemySeen = false;
            }
        }
        protected void OnPickableSensed(PickableController pickable)
        {
          if (pickable.IsWeapon() && )
          {
            
          }
        }

        protected void OnPickableUnsensed(PickableController pickable)
        {
          nani.xd;
        }
        private enum State
        {
          Seeking,
          Shooting,
          PickingWeapon
        }
    }
}
