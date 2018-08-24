﻿using System;
using Playmode.Pickables.Types;
using Playmode.Entity.Senses;
using UnityEngine;
using Playmode.Ennemy;
using Playmode.Entity.Destruction;

namespace Playmode.Pickables
{
    public class PickableController : MonoBehaviour
    {

        [Header("Type Images")] [SerializeField] private Sprite medkitSprite;
        [SerializeField] private Sprite shotgunSprite;
        [SerializeField] private Sprite uziSprite;
        [Header("Values")] [SerializeField] private int hitpoints;

        private EnnemySensor ennemySensor;
        private Destroyer destroyer;
        private PickableTypes type;

        private void Awake()
        {
            ValidateSerializeFields();
            InitializeComponents();
        }

        private void OnEnable()
        {
            ennemySensor.OnEnnemySensed += OnEnnemySensed;
        }

        private void OnDisable()
        {
            ennemySensor.OnEnnemySensed -= OnEnnemySensed;
        }

        private void ValidateSerializeFields()
        {
            if (medkitSprite == null)
                throw new ArgumentException("Type sprites must be provided. Medkit is missing.");
            if (shotgunSprite == null)
                throw new ArgumentException("Type sprites must be provided. Shotgun is missing.");
            if (uziSprite == null)
                throw new ArgumentException("Type sprites must be provided. Uzi is missing.");
        }

        private void InitializeComponents()
        {
            destroyer = GetComponent<RootDestroyer>();
            var rootTransform = transform.root;
            ennemySensor = rootTransform.GetComponent<EnnemySensor>();
        }

        public void Configure(PickableTypes type)
        {
            switch (type)
            {
                case PickableTypes.Medkit:
                    transform.root.GetComponent<SpriteRenderer>().sprite = medkitSprite;
                    break;
                case PickableTypes.Shotgun:
                    transform.root.GetComponent<SpriteRenderer>().sprite = shotgunSprite;
                    break;
                case PickableTypes.Uzi:
                    transform.root.GetComponent<SpriteRenderer>().sprite = uziSprite;
                    break;
            }
        }

        public void OnEnnemySensed(EnnemyController ennemy)
        {
            ennemy.Heal(hitpoints);
            destroyer.Destroy();
        }
    }
}
