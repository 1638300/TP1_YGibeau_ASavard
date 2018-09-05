using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CamperStrategy : BaseStrategy
{
    private const float closestDistanceAllowedMedkit = 3.0f;
    private State state = State.Seeking;

    private Health health;

    public CamperStrategy(
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
                ShootTowardsEnnemy();
                break;
            case State.PickingWeapon:
                MoveTowardsWeapon();
                break;
            case State.SearchingEnnemy:
                SearchEnnemy();
                break;
        }
    }

    protected override void OnEnnemySensed(EnnemyController ennemy)
    {
        if (state == State.SearchingEnnemy)
        {
            state = State.Shooting;
        }
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (state == State.Shooting && ennemySensor.GetFirstEnnemy == null)
        {
            state = State.SearchingEnnemy;
        }
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        if (state == State.Seeking && pickable.IsWeapon())
        {
            state = State.PickingWeapon;
        }
        else if(pickable.IsMedkit() && state != State.PickingMedkit && state != State.SearchingEnnemy && state != State.Shooting)
        {
            state = State.PickingMedkit;
        }
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        if (state != State.Seeking && state != State.PickingWeapon && base.pickableSensor.GetFirstMedkit == null)
        {
            if (base.pickableSensor.GetFirstWeapon == null)
                state = State.Seeking;
            else
                state = State.PickingWeapon;
        }
        else if (state == State.PickingWeapon && base.pickableSensor.GetFirstWeapon == null)
        {
            state = State.Seeking;
        }
    }

    private void OnLowLife()
    {
        if (base.pickableSensor.GetFirstMedkit != null)
        {
            state = State.PickingMedkit;
        }
    }

    private void ShootTowardsEnnemy()
    {
        if (ennemySensor.GetFirstEnnemy != null)
        {
            Vector3 ennemyPosition = ennemySensor.GetFirstEnnemy.transform.position;
            mover.RotateTowards(ennemyPosition);
            handController.Use();
        }
    }

    private void MoveTowardsWeapon()
    {
        if (pickableSensor.GetFirstWeapon != null)
        {
            Vector3 weaponPosition = pickableSensor.GetFirstWeapon.transform.position;
            mover.RotateTowards(weaponPosition);
            mover.Move(Mover.Foward);
        }
    }

    private void MoveTowardsMedkit()
    {
        if (pickableSensor.GetFirstMedkit != null)
        {
            Vector3 medkitPosition = pickableSensor.GetFirstMedkit.transform.position;

            if (Vector3.Distance(medkitPosition, mover.transform.position) > closestDistanceAllowedMedkit || health.IsLowLife)
            {
                mover.RotateTowards(medkitPosition);
                mover.Move(Mover.Foward);
            }
            else
                state = State.SearchingEnnemy;
        }
        else
            state = State.Seeking;
    }

    private void SearchEnnemy()
    {
        mover.Rotate(Mover.Clockwise);
    }

    private enum State
    {
        Seeking,
        PickingMedkit,
        SearchingEnnemy,
        Shooting,
        PickingWeapon
    }
}
