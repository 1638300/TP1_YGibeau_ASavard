using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CarefulStrategy : BaseStrategy
{
    private const int closestDistanceAllowed = 8;
    private State state = State.Seeking;

    public CarefulStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor worldSensor, 
                            EnnemySensor ennemySensor,
                            PickableSensor pickableSensor) 
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
            case State.SeekingMedicalKit:
                break;
            case State.Shooting:
                RunAndGun();
                break;
            case State.PickingWeapon:
                break;
        }
    }

    protected override void OnEnnemySensed(EnnemyController ennemy)
    {
        state = State.Shooting;
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (ennemySensor.GetFirstEnnemy == null)
        {
            state = State.Seeking;
        }
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        //throw new System.NotImplementedException();
    }

    private void RunAndGun()
    {
        Vector3 position = ennemySensor.GetFirstEnnemy.transform.position;
        mover.RotateTowards(position);

        if (Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
            mover.Move(Mover.Foward);
        else if (Vector3.Distance(position, mover.transform.position) < closestDistanceAllowed)
            mover.Move(Mover.Backward);

        handController.Use();
    }

    private enum State
    {
        Seeking,
        SeekingMedicalKit,
        Shooting,
        PickingWeapon
    }
}
