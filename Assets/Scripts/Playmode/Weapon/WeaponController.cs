using Playmode.Bullet;
using System;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {


        [Header("Behaviour")] [SerializeField] private GameObject bulletPrefab;
        [SerializeField] protected float fireDelayInSeconds = 0.5f;
        [SerializeField] private int uziDefaultDamage = 8;
        [SerializeField] private float uziDefaultFireRate = 0.2f;
        [SerializeField] private float uziBonusFireRate = 0.05f;
        [SerializeField] private int shotgunDefaultDamage = 14;
        [SerializeField] private int shotgunBonusDamage = 1;
        [SerializeField] private float shotgunDefaultFireRate = 2f;
        [SerializeField] private int shotgunNbBullets = 5;
        [SerializeField] private int shotgunBulletSpacing = 20;
        [SerializeField] private int shotgunBulletSpreading = 40;
        
        private float lastTimeShotInSeconds;

        private bool CanShoot => Time.time - lastTimeShotInSeconds > fireDelayInSeconds;

        [SerializeField] private int damage = 10;

        private WeaponType weaponType = WeaponType.Default;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
        }

        private void ValidateSerialisedFields()
        {
            if (fireDelayInSeconds < 0)
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
                        bulletObject.transform.GetComponentInChildren<BulletController>().SetDamage(damage);
                        bulletObject.transform.Rotate(Vector3.forward, (shotgunBulletSpacing * i - shotgunBulletSpreading));
                    }
                }
                else
                   Instantiate(bulletPrefab, transform.position, transform.rotation).transform.GetComponentInChildren<BulletController>().SetDamage(damage); ;

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
                  fireDelayInSeconds = shotgunDefaultFireRate;
              }
              else
              {
                  damage = uziDefaultDamage;
                  fireDelayInSeconds = uziDefaultFireRate;
              }
          }
          else if (weaponType == WeaponType.Shotgun)
          {
             damage += shotgunBonusDamage;
          }
          else
          {
             fireDelayInSeconds -= uziBonusFireRate;
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