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
        public Rigidbody _rigidbody;
        public BulletType bulletType;
        public float damageValue;
        public float timerSet = 2;
        private float timer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }

        void OnEnable()
        {
            timer = timerSet;
        }

        void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                BulletPool.Instance.Release(gameObject);
            }
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
                default:
                    break;
            }

            BulletPool.Instance.Release(gameObject);
        }
    }
}