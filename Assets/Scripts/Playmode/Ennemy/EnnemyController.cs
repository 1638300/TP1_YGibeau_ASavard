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
        [Header("Body Parts")] [SerializeField] private GameObject body;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject sight;
        [SerializeField] private GameObject typeSign;
        [Header("Type Images")] [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite carefulSprite;
        [SerializeField] private Sprite cowboySprite;
        [SerializeField] private Sprite camperSprite;
        [Header("Behaviour")] [SerializeField] private GameObject startingWeaponPrefab;
        [SerializeField] private int lowLifeThreshold;
        [SerializeField] private WorldSensor frontWorldSensor;
        [SerializeField] private WorldSensor backWorldSensor;

        public event LowLifeEventHandler OnLowLife;
        public event LowLifeEventHandler OnNormalLife;

        private Health health;
        private Mover mover;
        private Destroyer destroyer;
        private EnnemySensor ennemySensor;
        private PickableSensor pickableSensor;
        private HitSensor hitSensor;
        private HandController handController;
        private IEnnemyStrategy strategy;
        private bool isLowLife;
        
        public bool IsLowLife
        {
            get
            {
                return isLowLife;
            }
            private set
            {
                isLowLife = value;
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
            if (body == null)
                throw new ArgumentException("Body parts must be provided. Body is missing.");
            if (hand == null)
                throw new ArgumentException("Body parts must be provided. Hand is missing.");
            if (sight == null)
                throw new ArgumentException("Body parts must be provided. Sight is missing.");
            if (typeSign == null)
                throw new ArgumentException("Body parts must be provided. TypeSign is missing.");
            if (normalSprite == null)
                throw new ArgumentException("Type sprites must be provided. Normal is missing.");
            if (carefulSprite == null)
                throw new ArgumentException("Type sprites must be provided. Careful is missing.");
            if (cowboySprite == null)
                throw new ArgumentException("Type sprites must be provided. Cowboy is missing.");
            if (camperSprite == null)
                throw new ArgumentException("Type sprites must be provided. Camper is missing.");
            if (startingWeaponPrefab == null)
                throw new ArgumentException("StartingWeapon prefab must be provided.");
        }

        private void InitializeComponent()
        {
            health = GetComponent<Health>();
            mover = GetComponent<RootMover>();
            destroyer = GetComponent<RootDestroyer>();

            var rootTransform = transform.root;
            ennemySensor = rootTransform.GetComponentInChildren<EnnemySensor>();
            pickableSensor = rootTransform.GetComponentInChildren<PickableSensor>();
            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            handController = hand.GetComponent<HandController>();
        }

        private void CreateStartingWeapon()
        {
            handController.Hold(Instantiate(
                startingWeaponPrefab,
                Vector3.zero,
                Quaternion.identity
            ));
        }

        private void OnEnable()
        {
            hitSensor.OnHit += OnHit;
            health.OnDeath += OnDeath;
        }

        private void Update()
        {
            strategy.Act();
        }

        private void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
            health.OnDeath -= OnDeath;
        }

        public void Configure(EnnemyStrategy strategy, Color color)
        {
            body.GetComponent<SpriteRenderer>().color = color;
            sight.GetComponent<SpriteRenderer>().color = color;
            
            switch (strategy)
            {
                case EnnemyStrategy.Careful:
                    typeSign.GetComponent<SpriteRenderer>().sprite = carefulSprite;
                    this.strategy = new CarefulStrategy(
                                                        mover, 
                                                        handController, 
                                                        frontWorldSensor, 
                                                        backWorldSensor, 
                                                        ennemySensor, 
                                                        pickableSensor, 
                                                        this);
                    break;
                case EnnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    this.strategy = new CowboyStrategy(
                                                        mover,
                                                        handController,
                                                        frontWorldSensor,
                                                        backWorldSensor,
                                                        ennemySensor,
                                                        pickableSensor);
                    break;
                case EnnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    this.strategy = new CamperStrategy(
                                                        mover,
                                                        handController,
                                                        frontWorldSensor,
                                                        backWorldSensor,
                                                        ennemySensor,
                                                        pickableSensor,
                                                        this);
                    break;
                default:
                    typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
                    this.strategy = new NormalStrategy(
                                                        mover,
                                                        handController,
                                                        frontWorldSensor,
                                                        backWorldSensor,
                                                        ennemySensor,
                                                        pickableSensor);
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
            health.Heal(hitpoints);
            if (health.HealthPoints > lowLifeThreshold)
            {
                IsLowLife = false;
                NotifyNormalLife();
            }
        }

        private void OnHit(int hitPoints)
        {
            health.Hit(hitPoints);
            if(health.HealthPoints < lowLifeThreshold)
            {
                IsLowLife = true;
                NotifyLowLife();
            }
        }

        private void OnDeath(EnnemyController ennemy)
        {
            Debug.Log("Yaaaaarggg....!! I died....GG.");

            destroyer.Destroy();
        }
    }
}