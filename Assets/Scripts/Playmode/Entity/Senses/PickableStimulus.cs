using Playmode.Pickables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class PickableStimulus : MonoBehaviour
    {
        private PickableController _pickable;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _pickable = transform.root.GetComponent<PickableController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<PickableSensor>()?.Sense(_pickable);
        }

        
    }
}
