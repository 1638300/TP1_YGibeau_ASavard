using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Senses;
using Playmode.Movement;
using Playmode.Pickables;
using UnityEngine;

public class CamperStrategy : BaseStrategy
{
    private const float CLOSEST_DISTANCE_ALLOWED_MEDKIT = 3.0f;

    private EnnemyController _ennemyController;
    private bool _isLowLife;
    private State _state = State.Seeking;

    public CamperStrategy(
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
        if (_state == State.SearchingEnnemy) _state = State.Shooting;
    }

    protected override void OnEnnemyUnsensed(EnnemyController ennemy)
    {
        if (_state == State.Shooting && EnnemySensor.GetFirstEnnemy == null) _state = State.SearchingEnnemy;
    }

    protected override void OnPickableSensed(PickableController pickable)
    {
        if (_state == State.Seeking && pickable.IsWeapon())
            _state = State.PickingWeapon;
        else if (pickable.IsMedkit() && _state != State.PickingMedkit && _state != State.SearchingEnnemy &&
                 _state != State.Shooting) _state = State.PickingMedkit;
    }

    protected override void OnPickableUnsensed(PickableController pickable)
    {
        if (_state != State.Seeking && _state != State.PickingWeapon && PickableSensor.GetFirstMedkit == null)
        {
            if (PickableSensor.GetFirstWeapon == null)
                _state = State.Seeking;
            else
                _state = State.PickingWeapon;
        }
        else if (_state == State.PickingWeapon && PickableSensor.GetFirstWeapon == null)
        {
            _state = State.Seeking;
        }
    }

    private void OnLowLife()
    {
        _isLowLife = true;
        if (PickableSensor.GetFirstMedkit != null) _state = State.PickingMedkit;
    }

    private void OnNormalLife()
    {
        _isLowLife = false;
    }

    private void ShootTowardsEnnemy()
    {
        if (EnnemySensor.GetFirstEnnemy != null)
        {
            var ennemyPosition = EnnemySensor.GetFirstEnnemy.transform.position;
            Mover.RotateTowards(ennemyPosition);
            HandController.Use();
        }
    }

    private void MoveTowardsWeapon()
    {
        if (PickableSensor.GetFirstWeapon != null)
        {
            var weaponPosition = PickableSensor.GetFirstWeapon.transform.position;
            Mover.RotateTowards(weaponPosition);
            Mover.Move(Mover.Foward);
        }
    }

    private void MoveTowardsMedkit()
    {
        if (PickableSensor.GetFirstMedkit != null)
        {
            var medkitPosition = PickableSensor.GetFirstMedkit.transform.position;

            if (Vector3.Distance(medkitPosition, Mover.transform.position) > CLOSEST_DISTANCE_ALLOWED_MEDKIT || _isLowLife)
            {
                Mover.RotateTowards(medkitPosition);
                Mover.Move(Mover.Foward);
            }
            else
            {
                _state = State.SearchingEnnemy;
            }
        }
        else
        {
            _state = State.Seeking;
        }
    }

    private void SearchEnnemy()
    {
        Mover.Rotate(Mover.CLOCKWISE);
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