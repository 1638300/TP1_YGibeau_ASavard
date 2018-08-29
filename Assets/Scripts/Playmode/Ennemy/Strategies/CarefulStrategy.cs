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
            case State.PickingMedkit:
                Vector3 medkitPosition = pickableSensor.GetFirstMedkit().transform.position;
                MoveTowardsMedkit(medkitPosition);
                break;
            case State.Shooting:
                Vector3 ennemyPosition = ennemySensor.GetFirstEnnemy.transform.position;
                MoveAndShootTowardsEnnemy(ennemyPosition);
                break;
            case State.PickingWeapon:
                Vector3 weaponPosition = pickableSensor.GetFirstWeapon().transform.position;
                MoveTowardsWeapon(weaponPosition);
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
        if (state != State.PickingMedkit && pickable.IsMedkit())
        {
            state = State.PickingMedkit;
        }
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        if (state == State.PickingWeapon && base.pickableSensor.GetFirstWeapon() == null)
        {
            if (base.ennemySensor.GetFirstEnnemy == null)
            {
                state = State.Seeking;
            }
            else
            {
                state = State.Shooting;
            }

        }
    }

    private void MoveAndShootTowardsEnnemy(Vector3 position)
    {
        mover.RotateTowards(position);

        if (Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
            mover.Move(Mover.Foward);
        else if (Vector3.Distance(position, mover.transform.position) < closestDistanceAllowed)
            mover.Move(Mover.Backward);

        handController.Use();
    }

    private void MoveTowardsMedkit(Vector3 position)
    {
        mover.RotateTowards(position);

        mover.Move(Mover.Foward);
    }

    private void MoveTowardsWeapon(Vector3 position)
    {
        mover.RotateTowards(position);

        mover.Move(Mover.Foward);
    }

    private enum State
    {
        Seeking,
        PickingMedkit,
        Shooting,
        PickingWeapon
    }
}
