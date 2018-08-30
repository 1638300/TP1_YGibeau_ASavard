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
    private bool isLowLife;

    private EnnemyController ennemyController;

    public CarefulStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor frontWorldSensor,
                            WorldSensor backWorldSensor,
                            EnnemySensor ennemySensor,
                            PickableSensor pickableSensor,
                            EnnemyController ennemyController) 
        : base(mover, handController, frontWorldSensor, backWorldSensor, ennemySensor, pickableSensor)
    {
        this.ennemyController = ennemyController;
        ennemyController.OnLowLife += OnLowLife;
        ennemyController.OnNormalLife += OnNormalLife;
    }

    ~CarefulStrategy()
    {
        ennemyController.OnLowLife -= OnLowLife;
        ennemyController.OnNormalLife -= OnNormalLife;
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
                MoveTowardsPickable(medkitPosition);
                break;
            case State.Shooting:
                Vector3 ennemyPosition = ennemySensor.GetFirstEnnemy.transform.position;
                MoveAndShootTowardsEnnemy(ennemyPosition);
                break;
            case State.PickingWeapon:
                Vector3 weaponPosition = pickableSensor.GetFirstWeapon().transform.position;
                MoveTowardsPickable(weaponPosition);
                break;
        }
    }

    protected override void OnEnnemySensed(EnnemyController ennemy)
    {
        if(state == State.Seeking && !isLowLife)
        {
            state = State.Shooting;
        }
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (state == State.Shooting && ennemySensor.GetFirstEnnemy == null)
        {
            state = State.Seeking;
        }
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        if (state == State.Seeking && pickable.IsMedkit())
        {
            state = State.PickingMedkit;
        }
        if(pickable.IsWeapon() && state == State.Seeking && !isLowLife)
        {
            state = State.PickingWeapon;
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

    private void OnLowLife()
    {
        if(!isLowLife)
        {
            isLowLife = true;
            base.mover.ExtremeSpeedActivated();
        }

        state = State.Seeking;
    }

    private void OnNormalLife()
    {
        isLowLife = false;
        state = State.Seeking;
    }

    private void MoveAndShootTowardsEnnemy(Vector3 position)
    {
        mover.RotateTowards(position);

        if (Vector3.Distance(position, mover.transform.position) > closestDistanceAllowed)
            mover.Move(Mover.Foward);
        else if (Vector3.Distance(position, mover.transform.position) < closestDistanceAllowed && !isWorldColliding)
            mover.Move(Mover.Backward);

        handController.Use();
    }

    private void MoveTowardsPickable(Vector3 position)
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
