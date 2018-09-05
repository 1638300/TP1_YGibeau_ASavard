﻿using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class NormalStrategy : BaseStrategy
    {
        private bool _isEnnemySeen;
        private const int CLOSEST_DISTANCE_ALLOWED = 3;


        public NormalStrategy(
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
            if (_isEnnemySeen)
            {
                Vector3 position = ennemySensor.GetFirstEnnemy.transform.position;
                mover.RotateTowards(position);
                if(Vector3.Distance(position, mover.transform.position) > CLOSEST_DISTANCE_ALLOWED)
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
            _isEnnemySeen = true;
        }

        protected override void OnEnnemyUnsensed(EnnemyController ennemy)
        {
            if(ennemySensor.GetFirstEnnemy == null)
            {
                _isEnnemySeen = false;
            }
        }

        protected override void OnPickableSensed(PickableController pickable){ }

        protected override void OnPickableUnsensed(PickableController pickable){ }
    }
}
