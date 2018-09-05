using System;
using Playmode.Entity.Destruction;
using Playmode.Movement;
using UnityEngine;

namespace Playmode.Bullet
{
    public class BulletController : MonoBehaviour
    {
        [Header("Behaviour")] [SerializeField] private float _lifeSpanInSeconds = 5f;

        private Mover _mover;
        private Destroyer _destroyer;
        private float _timeSinceSpawnedInSeconds;

        private bool IsAlive => _timeSinceSpawnedInSeconds < _lifeSpanInSeconds;
        public int Damage { get; set; } = 1;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitialzeComponent();
        }

        private void ValidateSerialisedFields()
        {
            if (_lifeSpanInSeconds < 0)
                throw new ArgumentException("LifeSpan can't be lower than 0.");
        }

        private void InitialzeComponent()
        {
            _mover = GetComponent<RootMover>();
            _destroyer = GetComponent<RootDestroyer>();

            _timeSinceSpawnedInSeconds = 0;
        }

        private void Update()
        {
            UpdateLifeSpan();

            Act();
        }

        private void UpdateLifeSpan()
        {
            _timeSinceSpawnedInSeconds += Time.deltaTime;
        }

        private void Act()
        {
            if (IsAlive)
                _mover.Move(Mover.Foward);
            else
                _destroyer.Destroy();
        }

    }
}