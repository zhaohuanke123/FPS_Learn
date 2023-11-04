using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vanks.FPS;

namespace Vanks
{
    public enum BulletType
    {
        Player = 0,
        Enemy = 1
    }

    public class BulletController : MonoBehaviour
    {
        public float startSpeed = 100;
        private Rigidbody _rigidbody;
        public BulletType bulletType;
        public float damageValue;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.velocity = transform.forward * startSpeed;
            Destroy(gameObject, 5);
        }

        void Update()
        {
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"{bulletType} Bullet Hit: {other.gameObject.name}");
            switch (bulletType)
            {
                case BulletType.Player:
                    if (other.gameObject.CompareTag("Enemy"))
                    {
                        other.gameObject.GetComponent<HealthController>().Damage(damageValue);
                    }

                    break;
                case BulletType.Enemy:
                    if (other.gameObject.CompareTag("Player"))
                    {
                        other.gameObject.GetComponent<HealthController>().Damage(damageValue);
                    }

                    break;
            }

            Destroy(gameObject);
        }
    }
}