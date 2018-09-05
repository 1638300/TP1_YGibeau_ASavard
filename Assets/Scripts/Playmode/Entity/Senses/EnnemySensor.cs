using System.Collections.Generic;
using Playmode.Ennemy;
using Playmode.Entity.Status;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemySensorEventHandler(EnnemyController ennemy);

    public class EnnemySensor : MonoBehaviour
    {
        private ICollection<EnnemyController> _ennemiesInSight;

        public event EnnemySensorEventHandler OnEnnemySensed;
        public event EnnemySensorEventHandler OnEnnemyUnsensed;

        public EnnemyController GetFirstEnnemy
        {
            get
            {
                var ennemy = _ennemiesInSight.GetEnumerator();
                ennemy.MoveNext();
                return ennemy.Current;
            }
        }

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _ennemiesInSight = new LinkedList<EnnemyController>();
        }

        public void Sense(EnnemyController ennemy)
        {
            if(!_ennemiesInSight.Contains(ennemy))
            {
                _ennemiesInSight.Add(ennemy);

                ennemy.GetComponent<Health>().OnDeath += OnDeath;

                NotifyEnnemySensed(ennemy);
            }
        }

        public void Unsense(EnnemyController ennemy)
        {
            _ennemiesInSight.Remove(ennemy);

            ennemy.GetComponent<Health>().OnDeath -= OnDeath;

            NotifyEnnemySightUnsensed(ennemy);
        }

        private void NotifyEnnemySensed(EnnemyController ennemy)
        {
            if (OnEnnemySensed != null) OnEnnemySensed(ennemy);
        }

        private void NotifyEnnemySightUnsensed(EnnemyController ennemy)
        {
            if (OnEnnemyUnsensed != null) OnEnnemyUnsensed(ennemy);
        }

        private void OnDeath(EnnemyController ennemy)
        {
            _ennemiesInSight.Remove(ennemy);
            this.OnEnnemyUnsensed(ennemy);
        }
    }
}