using Playmode.Ennemy;
using System;
using UnityEngine;

namespace Playmode.Entity.Status
{
    public delegate void HealthEventHandler(EnnemyController ennemy);
    public delegate void LowLifeEventHandler(); //BEN_CORRECTION : Utilisé pas juste pour LowLife. Pourquoi pas HealthChangedEventHandler ?

    public class Health : MonoBehaviour
    {
        [SerializeField] private int healthPoints = 100;
        [SerializeField] private int lowLifeThreshold = 30;

        public event HealthEventHandler OnDeath;
        public event LowLifeEventHandler OnLowLife;
        public event LowLifeEventHandler OnNormalLife;

        public bool IsLowLife { get; private set; }

        public int HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                healthPoints = value < 0 ? 0 : value;

                if (healthPoints <= 0) NotifyDeath();
            }
        }

        private void Awake()
        {
            ValidateSerialisedFields();
        }

        private void ValidateSerialisedFields()
        {
            if (healthPoints < 0)
                throw new ArgumentException("HealthPoints can't be lower than 0.");
            if (lowLifeThreshold < 0)
                throw new ArgumentException("LowLifeThreshold can't be lower than 0.");
        }

        public void Hit(int hitPoints)
        {
            HealthPoints -= hitPoints;
            if (HealthPoints <= lowLifeThreshold)
            {
                IsLowLife = true;
                NotifyLowLife();
            }
        }

        public void Heal(int hitpoints)
        {
            HealthPoints += hitpoints;
            if (HealthPoints > lowLifeThreshold)
            {
                IsLowLife = false;
                NotifyNormalLife();
            }
                
        }

        private void NotifyDeath()
        {
            if (OnDeath != null) OnDeath(GetComponentInParent<EnnemyController>());
        }

        public void NotifyLowLife()
        {
            if (OnLowLife != null) OnLowLife();
        }

        public void NotifyNormalLife()
        {
            if (OnNormalLife != null) OnNormalLife();
        }
    }
}