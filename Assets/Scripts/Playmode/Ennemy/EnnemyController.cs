using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using System;
using UnityEngine;

namespace Playmode.Ennemy
{
    public delegate void LowLifeEventHandler();

    public class EnnemyController : MonoBehaviour
    {
        [Header("Body Parts")] [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _hand;
        [SerializeField] private GameObject _sight;
        [SerializeField] private GameObject _typeSign;
        [Header("Type Images")] [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _carefulSprite;
        [SerializeField] private Sprite _cowboySprite;
        [SerializeField] private Sprite _camperSprite;
        [Header("Behaviour")] [SerializeField] private GameObject _startingWeaponPrefab;
        [SerializeField] private int _lowLifeThreshold;
        [SerializeField] private WorldSensor _frontWorldSensor;
        [SerializeField] private WorldSensor _backWorldSensor;

        public event LowLifeEventHandler OnLowLife;
        public event LowLifeEventHandler OnNormalLife;

        private Health _health;
        private Mover _mover;
        private Destroyer _destroyer;
        private EnnemySensor _ennemySensor;
        private PickableSensor _pickableSensor;
        private HitSensor _hitSensor;
        private HandController _handController;
        private IEnnemyStrategy _strategy;
        private bool _isLowLife;
        
        public bool IsLowLife
        {
            get
            {
                return _isLowLife;
            }
            private set
            {
                _isLowLife = value;
            }
        }

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
            CreateStartingWeapon();
        }

        private void ValidateSerialisedFields()
        {
            if (_body == null)
                throw new ArgumentException("Body parts must be provided. Body is missing.");
            if (_hand == null)
                throw new ArgumentException("Body parts must be provided. Hand is missing.");
            if (_sight == null)
                throw new ArgumentException("Body parts must be provided. Sight is missing.");
            if (_typeSign == null)
                throw new ArgumentException("Body parts must be provided. TypeSign is missing.");
            if (_normalSprite == null)
                throw new ArgumentException("Type sprites must be provided. Normal is missing.");
            if (_carefulSprite == null)
                throw new ArgumentException("Type sprites must be provided. Careful is missing.");
            if (_cowboySprite == null)
                throw new ArgumentException("Type sprites must be provided. Cowboy is missing.");
            if (_camperSprite == null)
                throw new ArgumentException("Type sprites must be provided. Camper is missing.");
            if (_startingWeaponPrefab == null)
                throw new ArgumentException("StartingWeapon prefab must be provided.");
        }

        private void InitializeComponent()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<RootMover>();
            _destroyer = GetComponent<RootDestroyer>();

            var rootTransform = transform.root;
            _ennemySensor = rootTransform.GetComponentInChildren<EnnemySensor>();
            _pickableSensor = rootTransform.GetComponentInChildren<PickableSensor>();
            _hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            _handController = _hand.GetComponent<HandController>();
        }

        private void CreateStartingWeapon()
        {
            _handController.Hold(Instantiate(
                _startingWeaponPrefab,
                Vector3.zero,
                Quaternion.identity
            ));
        }

        private void OnEnable()
        {
            _hitSensor.OnHit += OnHit;
            _health.OnDeath += OnDeath;
        }

        private void Update()
        {
            _strategy.Act();
        }

        private void OnDisable()
        {
            _hitSensor.OnHit -= OnHit;
            _health.OnDeath -= OnDeath;
        }

        public void Configure(EnnemyStrategy strategy, Color color)
        {
            _body.GetComponent<SpriteRenderer>().color = color;
            _sight.GetComponent<SpriteRenderer>().color = color;
            
            switch (strategy)
            {
                case EnnemyStrategy.Careful:
                    _typeSign.GetComponent<SpriteRenderer>().sprite = _carefulSprite;
                    this._strategy = new CarefulStrategy(
                                                        _mover, 
                                                        _handController, 
                                                        _frontWorldSensor, 
                                                        _backWorldSensor, 
                                                        _ennemySensor, 
                                                        _pickableSensor, 
                                                        this);
                    break;
                case EnnemyStrategy.Cowboy:
                    _typeSign.GetComponent<SpriteRenderer>().sprite = _cowboySprite;
                    this._strategy = new CowboyStrategy(
                                                        _mover,
                                                        _handController,
                                                        _frontWorldSensor,
                                                        _backWorldSensor,
                                                        _ennemySensor,
                                                        _pickableSensor);
                    break;
                case EnnemyStrategy.Camper:
                    _typeSign.GetComponent<SpriteRenderer>().sprite = _camperSprite;
                    this._strategy = new CamperStrategy(
                                                        _mover,
                                                        _handController,
                                                        _frontWorldSensor,
                                                        _backWorldSensor,
                                                        _ennemySensor,
                                                        _pickableSensor,
                                                        this);
                    break;
                default:
                    _typeSign.GetComponent<SpriteRenderer>().sprite = _normalSprite;
                    this._strategy = new NormalStrategy(
                                                        _mover,
                                                        _handController,
                                                        _frontWorldSensor,
                                                        _backWorldSensor,
                                                        _ennemySensor,
                                                        _pickableSensor);
                    break;
            }
        }

        public void NotifyLowLife()
        {
            if (OnLowLife != null) OnLowLife();
        }

        public void NotifyNormalLife()
        {
            if (OnNormalLife != null) OnNormalLife();
        }

        public void Heal(int hitpoints)
        {
            _health.Heal(hitpoints);
            if (_health.HealthPoints > _lowLifeThreshold)
            {
                IsLowLife = false;
                NotifyNormalLife();
            }
        }

        private void OnHit(int hitPoints)
        {
            _health.Hit(hitPoints);
            if(_health.HealthPoints < _lowLifeThreshold)
            {
                IsLowLife = true;
                NotifyLowLife();
            }
        }

        private void OnDeath(EnnemyController ennemy)
        {
            _destroyer.Destroy();
        }
    }
}