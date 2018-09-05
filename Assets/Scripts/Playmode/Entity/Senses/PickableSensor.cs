using System.Collections.Generic;
using Playmode.Pickables;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void PickableSensorEventHandler(PickableController pickable);

    public class PickableSensor : MonoBehaviour
    {
        private ICollection<PickableController> _pickablesInSight;

        public event PickableSensorEventHandler OnPickableSensed;
        public event PickableSensorEventHandler OnPickableUnsensed;

        public IEnumerable<PickableController> PickablesInSight => _pickablesInSight;

        public PickableController GetFirstMedkit
        {
            get
            {
                foreach (PickableController pickableController in _pickablesInSight)
                {
                    if (pickableController.IsMedkit())
                    {
                        return pickableController;
                    }
                }
                return null;
            }
        }

        public PickableController GetFirstWeapon
        {
            get
            {
                foreach (PickableController pickableController in _pickablesInSight)
                {
                    if (pickableController.IsWeapon())
                    {
                        return pickableController;
                    }
                }
                return null;
            }
        }

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _pickablesInSight = new HashSet<PickableController>();
        }

        private void OnDisable()
        {
            foreach (PickableController pickableController in _pickablesInSight)
            {
                pickableController.OnDestroy -= Unsense;
            }
        }

        public void Sense(PickableController pickable)
        {
            _pickablesInSight.Add(pickable);

            pickable.OnDestroy += Unsense;

            NotifyPickableSensed(pickable);
        }

        public void Unsense(PickableController pickable)
        {
            _pickablesInSight.Remove(pickable);

            NotifyPickableUnsensed(pickable);
        }

        private void NotifyPickableSensed(PickableController pickable)
        {
            if (OnPickableSensed != null) OnPickableSensed(pickable);
        }

        private void NotifyPickableUnsensed(PickableController pickable)
        {
            if (OnPickableUnsensed != null) OnPickableUnsensed(pickable);
        }
    }
}
