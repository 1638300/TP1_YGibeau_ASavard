using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CarefulStrategy : BaseStrategy
{
    private const int CLOSEST_DISTANCE_ALLOWED = 8;
    private State state = State.Seeking;

    private Health health;

    public CarefulStrategy(
                            Mover mover, 
                            HandController handController, 
                            WorldSensor frontWorldSensor,
                            WorldSensor backWorldSensor,
                            EnnemySensor ennemySensor,
                            PickableSensor pickableSensor,
                            Health health) 
        : base(mover, handController, frontWorldSensor, backWorldSensor, ennemySensor, pickableSensor)
    {
        this.health = health;
        health.OnLowLife += OnLowLife;
        health.OnNormalLife += OnNormalLife;
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
        if(state == State.Seeking && !health.IsLowLife)
        {
            state = State.Shooting;
        }
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (state == State.Shooting && EnnemySensor.GetFirstEnnemy == null)
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
        if(pickable.IsWeapon() && state == State.Seeking && !health.IsLowLife)
        {
            state = State.PickingWeapon;
        }
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        if (base.PickableSensor.GetFirstMedkit == null && !health.IsLowLife)
        {
            if (base.EnnemySensor.GetFirstEnnemy == null)
            {
                if (base.PickableSensor.GetFirstWeapon == null)
                    state = State.Seeking;
                else
                    state = State.PickingWeapon;
            }
            else
                state = State.Shooting;
        }
        else if (state != State.Shooting || state != State.PickingWeapon || health.IsLowLife)
            state = State.PickingMedkit;
    }

    private void OnLowLife()
    {
        if(!health.IsLowLife)
        {
            base.Mover.ExtremeSpeedActivated();
            state = State.Seeking;
        }
    }

    private void OnNormalLife()
    {
        if(base.PickableSensor.GetFirstMedkit != null)
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
        if (EnnemySensor.GetFirstEnnemy != null)
        {
            Vector3 ennemyPosition = EnnemySensor.GetFirstEnnemy.transform.position;

            Mover.RotateTowards(ennemyPosition);

            if (Vector3.Distance(ennemyPosition, Mover.transform.position) > CLOSEST_DISTANCE_ALLOWED)
                Mover.Move(Mover.Foward);
            else if (Vector3.Distance(ennemyPosition, Mover.transform.position) < CLOSEST_DISTANCE_ALLOWED && !IsWorldColliding)
                Mover.Move(Mover.Backward);

            HandController.Use();
        }
    }

    private void MoveTowardsWeapon()
    {
        if(PickableSensor.GetFirstWeapon != null)
        {
            Vector3 weaponPosition = PickableSensor.GetFirstWeapon.transform.position;

            Mover.RotateTowards(weaponPosition);

            Mover.Move(Mover.Foward);
        }
    }

    private void MoveTowardsMedkit()
    {
        if(PickableSensor.GetFirstMedkit != null)
        {
            Vector3 medkitPosition = PickableSensor.GetFirstMedkit.transform.position;

            Mover.RotateTowards(medkitPosition);

            Mover.Move(Mover.Foward);
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
