using System;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.BodyParts
{
    public class HandController : MonoBehaviour
    {
        private Mover _mover;
        private WeaponController _weapon;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _mover = GetComponent<AnchoredMover>();
        }
        
        public void Hold(GameObject gameObject)
        {
            if (gameObject != null)
            {
                gameObject.transform.parent = transform;
                gameObject.transform.localPosition = Vector3.zero;
                
                _weapon = gameObject.GetComponentInChildren<WeaponController>();
            }
            else
            {
                _weapon = null;
            }
        }
        public void Hold(WeaponType newType)
        {
           _weapon?.SetWeaponType(newType);
        }

        public void Use()
        {
           if (_weapon != null) _weapon.Shoot();
        }
     }
}