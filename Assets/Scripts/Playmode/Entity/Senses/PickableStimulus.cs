using Playmode.Pickables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class PickableStimulus : MonoBehaviour
    {
        private PickableController pickable;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            pickable = transform.root.GetComponent<PickableController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collided with sensor");
            other.GetComponent<PickableSensor>()?.Sense(pickable);
        }

        
    }
}
