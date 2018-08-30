using Playmode.Bullet;
using System;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class HitStimulus : MonoBehaviour
    {

        private BulletController bulletController;

        private void Awake()
        {
            bulletController = transform.root.GetComponentInChildren<BulletController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<Entity.Senses.HitSensor>()?.Hit(bulletController.Damage);
        }
    }
}