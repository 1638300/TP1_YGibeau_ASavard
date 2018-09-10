using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class NormalStrategy : BaseStrategy
    {
        //BEN_REVIEW : SVP, classez vos éléments dans un ordre logique. La norme, c'est : constantes, serialized fields,
        //             attributs, propriétés, constructeurs, méthodes.
        private bool isEnnemySeen;
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
            if (isEnnemySeen)
            {
                Vector3 position = EnnemySensor.GetFirstEnnemy.transform.position;
                Mover.RotateTowards(position);
                //BEN_REVIEW : Une chose que j'aime bien faire, c'est extraire ces conditions dans des fonctions quand c'est
                //             trop long à lire.
                if(Vector3.Distance(position, Mover.transform.position) > CLOSEST_DISTANCE_ALLOWED)
                {
                    Mover.Move(Mover.Foward);
                }
                HandController.Use();
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
            if(EnnemySensor.GetFirstEnnemy == null)
            {
                isEnnemySeen = false;
            }
        }

        protected override void OnPickableSensed(PickableController pickable){ }

        protected override void OnPickableUnsensed(PickableController pickable){ }
    }
}
