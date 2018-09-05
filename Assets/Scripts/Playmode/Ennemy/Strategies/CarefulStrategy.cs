using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CarefulStrategy : BaseStrategy
{
    private const int CLOSEST_DISTANCE_ALLOWED = 8;
    private State _state = State.Seeking;
    private bool _isLowLife;

    private EnnemyController _ennemyController;

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
        this._ennemyController = ennemyController;
        ennemyController.OnLowLife += OnLowLife;
        ennemyController.OnNormalLife += OnNormalLife;
    }

    public override void Act()
    {
        switch (_state)
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
        if(_state == State.Seeking && !_isLowLife)
        {
            _state = State.Shooting;
        }
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (_state == State.Shooting && EnnemySensor.GetFirstEnnemy == null)
        {
            _state = State.Seeking;
        }
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        if (pickable.IsMedkit() && _state != State.Shooting)
        {
            _state = State.PickingMedkit;
        }
        if(pickable.IsWeapon() && _state == State.Seeking && !_isLowLife)
        {
            _state = State.PickingWeapon;
        }
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        if (base.PickableSensor.GetFirstMedkit == null && !_isLowLife)
        {
            if (base.EnnemySensor.GetFirstEnnemy == null)
            {
                if (base.PickableSensor.GetFirstWeapon == null)
                    _state = State.Seeking;
                else
                    _state = State.PickingWeapon;
            }
            else
                _state = State.Shooting;
        }
        else if (_state != State.Shooting || _state != State.PickingWeapon || _isLowLife)
            _state = State.PickingMedkit;
    }

    private void OnLowLife()
    {
        if(!_isLowLife)
        {
            _isLowLife = true;
            base.Mover.ExtremeSpeedActivated();
        }

        _state = State.Seeking;
    }

    private void OnNormalLife()
    {
        _isLowLife = false;
        if(base.PickableSensor.GetFirstMedkit != null)
        {
            _state = State.PickingMedkit;
        }
        else
        {
            _state = State.Seeking;
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
