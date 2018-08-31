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

        public PickableController GetFirstMedkit()
        {
          foreach (PickableController pickableController in pickablesInSight)
          {
            if (pickableController.IsMedkit())
            {
              return pickableController;
            }
          }
          return null;
        }

        public PickableController GetFirstWeapon()
        {
          foreach (PickableController pickableController in pickablesInSight)
          {
            if (pickableController.IsWeapon())
            {
              return pickableController;
            }
          }
          return null;
        }

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            pickablesInSight = new HashSet<PickableController>();
        }

        private void OnDisable()
        {
            foreach (PickableController pickableController in pickablesInSight)
            {
                pickableController.onDestroy -= Unsense;
            }
        }

        public void Sense(PickableController pickable)
        {
            pickablesInSight.Add(pickable);

            pickable.onDestroy += Unsense;

            NotifyPickableSensed(pickable);
        }

        public void Unsense(PickableController pickable)
        {
            pickablesInSight.Remove(pickable);

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
