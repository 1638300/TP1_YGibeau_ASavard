using System.Collections.Generic;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void PickableSensorEventHandler(PickableController pickable);

    public class PickableSensor : MonoBehaviour
    {
        private ICollection<PickableController> pickablesInSight;

        public event PickableSensorEventHandler OnPickableSensed;
        public event PickableSensorEventHandler OnPickableUnsensed;

        public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            pickablesInSight = new HashSet<PickableController>();
        }

        public void Sense(PickableController pickable)
        {
            pickablesInSight.Add(pickable);

            NotifyEnnemySensed(pickable);
        }

        public void Unsense(PickableController pickable)
        {
            pickablesInSight.Remove(pickable);

            NotifyEnnemySightUnsensed(pickable);
        }

        private void NotifyEnnemySensed(PickableController pickable)
        {
            if (OnPickableSensed != null) OnPickableSensed(pickable);
        }

        private void NotifyEnnemySightUnsensed(PickableController pickable)
        {
            if (OnPickableUnsensed != null) OnPickableUnsensed(pickable);
        }
    }
}
