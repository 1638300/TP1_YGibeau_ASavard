using System.Collections.Generic;
using Playmode.Ennemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemySensorEventHandler(EnnemyController ennemy);

    public class EnnemySensor : MonoBehaviour
    {
        private ICollection<EnnemyController> ennemiesInSight;

        public event EnnemySensorEventHandler OnEnnemySensed;
        public event EnnemySensorEventHandler OnEnnemyUnsensed;

        public IEnumerable<EnnemyController> EnnemiesInSight => ennemiesInSight;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ennemiesInSight = new HashSet<EnnemyController>();
        }

        public void Sense(EnnemyController ennemy)
        {
            ennemiesInSight.Add(ennemy);

            NotifyEnnemySensed(ennemy);
        }

        public void Unsense(EnnemyController ennemy)
        {
            ennemiesInSight.Remove(ennemy);

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
    }
}