using Playmode.Ennemy;
using System;
using UnityEngine;

namespace Playmode.Entity.Status
{
    public delegate void HealthEventHandler(EnnemyController ennemy);

    public class Health : MonoBehaviour
    {
        [SerializeField] private int _healthPoints = 100;

        public event HealthEventHandler OnDeath;

        public int HealthPoints
        {
            get { return _healthPoints; }
            private set
            {
                _healthPoints = value < 0 ? 0 : value;

                if (_healthPoints <= 0) NotifyDeath();
            }
        }

        private void Awake()
        {
            ValidateSerialisedFields();
        }

        private void ValidateSerialisedFields()
        {
            if (_healthPoints < 0)
                throw new ArgumentException("HealthPoints can't be lower than 0.");
        }

        public void Hit(int hitPoints)
        {
            HealthPoints -= hitPoints;
        }

        public void Heal(int hitpoints)
        {
            HealthPoints += hitpoints;
        }

        private void NotifyDeath()
        {
            if (OnDeath != null) OnDeath(GetComponentInParent<EnnemyController>());
        }
    }
}