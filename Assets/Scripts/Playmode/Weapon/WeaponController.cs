using Playmode.Bullet;
using System;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {


        [Header("Behaviour")] [SerializeField] private GameObject bulletPrefab;
        [SerializeField] protected float FireDelayInSeconds = 0.5f;
        [SerializeField] private int uziDefaultDamage = 10;
        [SerializeField] private float uziDefaultFireRate = 0.15f;
        [SerializeField] private float uziBonusFireRate = 0.05f;
        [SerializeField] private int shotgunDefaultDamage = 14;
        [SerializeField] private int shotgunBonusDamage = 1;
        [SerializeField] private float shotgunDefaultFireRate = 1.5f;
        [SerializeField] private int shotgunNbBullets = 5;
        [SerializeField] private int shotgunBulletSpacing = 20;
        [SerializeField] private int shotgunBulletSpreading = 40;
        
        private float lastTimeShotInSeconds;
        private float fireDelayBonus = 0;

        private bool CanShoot => Time.time - lastTimeShotInSeconds > FireDelayInSeconds;

        [SerializeField] private int damage = 10;

        private int bonusDamage = 0;

        private WeaponType weaponType = WeaponType.Default;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
        }

        private void ValidateSerialisedFields()
        {
            if (FireDelayInSeconds < 0)
                throw new ArgumentException("FireRate can't be lower than 0.");
        }

        private void InitializeComponent()
        {
            lastTimeShotInSeconds = 0;
        }

        public void Shoot()
        {
            if (CanShoot)
            {
                if (weaponType == WeaponType.Shotgun)
                {
                    for (int i = 0; i < shotgunNbBullets; i++)
                    {
                        var bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
                        bulletObject.transform.GetComponentInChildren<BulletController>().Damage = damage;
                        bulletObject.transform.Rotate(Vector3.forward, (shotgunBulletSpacing * i - shotgunBulletSpreading));
                    }
                }
                else
                   Instantiate(bulletPrefab, transform.position, transform.rotation).transform.GetComponentInChildren<BulletController>().Damage = damage;

                lastTimeShotInSeconds = Time.time;
            }
        }

        public void SetWeaponType(WeaponType type)
        {        
          if (weaponType == WeaponType.Default)
          {
              if (type == WeaponType.Shotgun)
              {
                  damage = shotgunDefaultDamage;
                  FireDelayInSeconds = shotgunDefaultFireRate;
              }
              else
              {
                  damage = uziDefaultDamage;
                  FireDelayInSeconds = uziDefaultFireRate;
              }
          }
          else if (type == WeaponType.Shotgun)
          {
             bonusDamage += shotgunBonusDamage;
             damage = shotgunDefaultDamage + bonusDamage;
             FireDelayInSeconds = shotgunDefaultFireRate - fireDelayBonus;
          }
          else
          {
             fireDelayBonus += uziBonusFireRate;
             damage = uziDefaultDamage + bonusDamage;
             FireDelayInSeconds = uziDefaultFireRate - fireDelayBonus;

          }
          weaponType = type;  
        }
    }
    public enum WeaponType
    {
      Default,
      Uzi,
      Shotgun
    }
}