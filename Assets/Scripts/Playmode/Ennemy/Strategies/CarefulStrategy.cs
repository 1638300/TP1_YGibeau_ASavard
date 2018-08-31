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
                MoveTowardsMedkit();
                break;
            case State.Shooting:
                MoveAndShootTowardsEnnemy();
                break;
            case State.PickingWeapon:
                MoveTowardsWeapon();
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
        if (pickable.IsMedkit() && state != State.Shooting)
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
        if (base.pickableSensor.GetFirstMedkit == null && !isLowLife)
        {
            if (base.ennemySensor.GetFirstEnnemy == null)
            {
                if (base.pickableSensor.GetFirstWeapon == null)
                    state = State.Seeking;
                else
                    state = State.PickingWeapon;
            }
            else
                state = State.Shooting;
        }
        else if (state != State.Shooting || state != State.PickingWeapon || isLowLife)
            state = State.PickingMedkit;
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
        if(base.pickableSensor.GetFirstMedkit != null)
        {
            state = State.PickingMedkit;
        }
        else
        {
            state = State.Seeking;
        }
        
    }

    private void MoveAndShootTowardsEnnemy()
    {
        if (ennemySensor.GetFirstEnnemy != null)
        {
            Vector3 ennemyPosition = ennemySensor.GetFirstEnnemy.transform.position;

            mover.RotateTowards(ennemyPosition);

            if (Vector3.Distance(ennemyPosition, mover.transform.position) > closestDistanceAllowed)
                mover.Move(Mover.Foward);
            else if (Vector3.Distance(ennemyPosition, mover.transform.position) < closestDistanceAllowed && !isWorldColliding)
                mover.Move(Mover.Backward);

            handController.Use();
        }
    }

    private void MoveTowardsWeapon()
    {
        if(pickableSensor.GetFirstWeapon != null)
        {
            Vector3 weaponPosition = pickableSensor.GetFirstWeapon.transform.position;

            mover.RotateTowards(weaponPosition);

            mover.Move(Mover.Foward);
        }
    }

    private void MoveTowardsMedkit()
    {
        if(pickableSensor.GetFirstMedkit != null)
        {
            Vector3 medkitPosition = pickableSensor.GetFirstMedkit.transform.position;

            mover.RotateTowards(medkitPosition);

            mover.Move(Mover.Foward);
        }
    }

    private enum State
    {
        Seeking,
        PickingMedkit,
        Shooting,
        PickingWeapon
    }
}
