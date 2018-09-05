using Playmode.Ennemy;
using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Pickables.Types;
using Playmode.Weapon;
using System;
using UnityEngine;

namespace Playmode.Pickables
{
    public delegate void PickableControllerEventHandler(PickableController pickableController);
    
    public class PickableController : MonoBehaviour
    {

        [Header("Type Images")] [SerializeField] private Sprite _medkitSprite;
        [SerializeField] private Sprite _shotgunSprite;
        [SerializeField] private Sprite _uziSprite;
        [Header("Values")] [SerializeField] private int _hitpoints;

        private EnnemySensor _ennemySensor;
        private Destroyer _destroyer;
        private PickableTypes _type;
        public event PickableControllerEventHandler OnDestroy;

        private void NotifyPickableDestroyed()
        {
            if (OnDestroy != null) OnDestroy(this);
        }

        private void Awake()
        {
            ValidateSerializeFields();
            InitializeComponents();
        }

        private void OnEnable()
        {
            _ennemySensor.OnEnnemySensed += OnEnnemySensed;
         }

        private void OnDisable()
        {
            _ennemySensor.OnEnnemySensed -= OnEnnemySensed;
        }

        private void ValidateSerializeFields()
        {
            if (_medkitSprite == null)
                throw new ArgumentException("Type sprites must be provided. Medkit is missing.");
            if (_shotgunSprite == null)
                throw new ArgumentException("Type sprites must be provided. Shotgun is missing.");
            if (_uziSprite == null)
                throw new ArgumentException("Type sprites must be provided. Uzi is missing.");
            if(_hitpoints < 0)
                throw new ArgumentException("_hitpoints can't be lower than 0.");
        }

        private void InitializeComponents()
        {
            _destroyer = GetComponent<RootDestroyer>();
            var rootTransform = transform.root;
            _ennemySensor = rootTransform.GetComponent<EnnemySensor>();
        }

        public void Configure(PickableTypes type)
        {
            switch (type)
            {
                case PickableTypes.Medkit:
                    transform.root.GetComponent<SpriteRenderer>().sprite = _medkitSprite;
                    this._type = PickableTypes.Medkit;
                    break;
                case PickableTypes.Shotgun:
                    transform.root.GetComponent<SpriteRenderer>().sprite = _shotgunSprite;
                    this._type = PickableTypes.Shotgun;
                    break;
                case PickableTypes.Uzi:
                    transform.root.GetComponent<SpriteRenderer>().sprite = _uziSprite;
                    this._type = PickableTypes.Uzi;
                    break;
            }
        }

        public void OnEnnemySensed(EnnemyController ennemy)
        {
            switch (_type)
            {
                case PickableTypes.Medkit:
                    ennemy.Heal(_hitpoints);
                    NotifyPickableDestroyed();
                    _destroyer.Destroy();
                    break;
                case PickableTypes.Shotgun:
                     ennemy.transform.root.GetComponentInChildren<HandController>().Hold(WeaponType.Shotgun);
                     NotifyPickableDestroyed();
                     _destroyer.Destroy();
                    break;
                case PickableTypes.Uzi:
                    ennemy.transform.root.GetComponentInChildren<HandController>().Hold(WeaponType.Uzi);
                    NotifyPickableDestroyed();
                    _destroyer.Destroy();
                    break;
            }
        }
        
        public bool IsMedkit()
        {
          return ((int)_type & (int)PickableTypes.Util) == (int)PickableTypes.Util;
        }

        public bool IsWeapon()
        {
          return ((int)_type & (int)PickableTypes.Weapon) == (int)PickableTypes.Weapon;
        }
    }
}
