using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CarefulStrategy : BaseStrategy
{
    public CarefulStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor worldSensor, 
                            EnnemySensor ennemySensor,
                            PickableSensor pickableSensor) 
        : base(mover, handController, worldSensor, ennemySensor, pickableSensor)
    {

    }

    protected override void OnEnnemySensed(EnnemyController ennemy)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        throw new System.NotImplementedException();
    }
}
