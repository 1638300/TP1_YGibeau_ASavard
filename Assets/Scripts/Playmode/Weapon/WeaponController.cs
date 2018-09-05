using Playmode.Bullet;
using System;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Behaviour")] [SerializeField] private GameObject _bulletPrefab;
        [Header("Global stats")][SerializeField] protected float FireDelayInSeconds = 0.5f;
        [SerializeField] private int _damage = 10;
        [Header("Uzi stats")][SerializeField] private int _uziDefaultDamage = 10;
        [SerializeField] private float _uziDefaultFireRate = 0.15f;
        [SerializeField] private float _uziBonusFireRate = 0.05f;
        [Header("Shotgun stats")][SerializeField] private int _shotgunDefaultDamage = 14;
        [SerializeField] private int _shotgunBonusDamage = 1;
        [SerializeField] private float _shotgunDefaultFireRate = 1.5f;
        [SerializeField] private int _shotgunNbBullets = 5;
        [SerializeField] private int _shotgunBulletSpacing = 20;
        [SerializeField] private int _shotgunBulletSpreading = 40;
        
        private float _lastTimeShotInSeconds;
        private float _fireDelayBonus = 0;

        private bool CanShoot => Time.time - _lastTimeShotInSeconds > FireDelayInSeconds;

        private int _bonusDamage = 0;

        private WeaponType _weaponType = WeaponType.Default;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
        }

        private void ValidateSerialisedFields()
        {
            if(_bulletPrefab == null)
                throw new ArgumentException("_bulletPrefab must be provided.");
            if (FireDelayInSeconds < 0)
                throw new ArgumentException("FireRate can't be lower than 0.");
            if (_damage < 0)
                throw new ArgumentException("_damage can't be lower than 0.");
            if (_uziDefaultDamage < 0)
                throw new ArgumentException("_uziDefaultDamage can't be lower than 0.");
            if (_uziDefaultFireRate < 0)
                throw new ArgumentException("_uziDefaultFireRate can't be lower than 0.");
            if (_uziBonusFireRate < 0)
                throw new ArgumentException("_uziBonusFireRate can't be lower than 0.");
            if (_shotgunDefaultDamage < 0)
                throw new ArgumentException("_shotgunDefaultDamage can't be lower than 0.");
            if (_shotgunBonusDamage < 0)
                throw new ArgumentException("_shotgunBonusDamage can't be lower than 0.");
            if (_shotgunDefaultFireRate < 0)
                throw new ArgumentException("_shotgunDefaultFireRate can't be lower than 0.");
            if (_shotgunNbBullets < 0)
                throw new ArgumentException("_shotgunNbBullets can't be lower than 0.");
            if (_shotgunBulletSpacing < 0)
                throw new ArgumentException("_shotgunBulletSpacing can't be lower than 0.");
            if (_shotgunBulletSpreading < 0)
                throw new ArgumentException("_shotgunBulletSpreading can't be lower than 0.");
        }

        private void InitializeComponent()
        {
            _lastTimeShotInSeconds = 0;
        }

        public void Shoot()
        {
            if (CanShoot)
            {
                if (_weaponType == WeaponType.Shotgun)
                {
                    for (int i = 0; i < _shotgunNbBullets; i++)
                    {
                        var bulletObject = Instantiate(_bulletPrefab, transform.position, transform.rotation);
                        bulletObject.transform.GetComponentInChildren<BulletController>().Damage = _damage;
                        bulletObject.transform.Rotate(Vector3.forward, (_shotgunBulletSpacing * i - _shotgunBulletSpreading));
                    }
                }
                else
                   Instantiate(_bulletPrefab, transform.position, transform.rotation).transform.GetComponentInChildren<BulletController>().Damage = _damage;

                _lastTimeShotInSeconds = Time.time;
            }
        }

        public void SetWeaponType(WeaponType type)
        {        
          if (_weaponType == WeaponType.Default)
          {
              if (type == WeaponType.Shotgun)
              {
                  _damage = _shotgunDefaultDamage;
                  FireDelayInSeconds = _shotgunDefaultFireRate;
              }
              else
              {
                  _damage = _uziDefaultDamage;
                  FireDelayInSeconds = _uziDefaultFireRate;
              }
          }
          else if (type == WeaponType.Shotgun)
          {
             _bonusDamage += _shotgunBonusDamage;
             _damage = _shotgunDefaultDamage + _bonusDamage;
             FireDelayInSeconds = _shotgunDefaultFireRate - _fireDelayBonus;
          }
          else
          {
             _fireDelayBonus += _uziBonusFireRate;
             _damage = _uziDefaultDamage + _bonusDamage;
             FireDelayInSeconds = _uziDefaultFireRate - _fireDelayBonus;

          }
          _weaponType = type;  
        }
    }
    public enum WeaponType
    {
      Default,
      Uzi,
      Shotgun
    }
}