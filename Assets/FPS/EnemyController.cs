using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Vanks.FPS
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        public GameObject player;
        public PlayerController pc;
        public bool isCheckPlayer = false;
        public float checkRadius = 10;
        public LayerMask playerLayer;

        public Transform bulletSpawnPoint;
        public GameObject bullet;
        public float minAngle = 30;
        public bool isFire;
        public float fireInterval;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            player = GameObject.FindGameObjectWithTag("Player");
            pc = player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            FireControl();
        }

        private void FixedUpdate()
        {
            DoCheck();
        }

        private void DoCheck()
        {
            isCheckPlayer = Physics.CheckSphere(transform.position, checkRadius, playerLayer);
        }

        private void FireControl()
        {
            // 1 检测是否位于射击范围内
            var distance = (pc.eyeViewTrans.position - bulletSpawnPoint.position).normalized;
            var angle = Vector3.Angle(bulletSpawnPoint.forward, distance);
            // Debug.Log($"angle: {angle}");
            if (angle < minAngle)
            {
                if (!isFire)
                {
                    StartCoroutine("Fire", distance);
                    isFire = true;
                }
            }
            else
            {
                if (isFire)
                {
                    StopCoroutine("Fire");
                    isFire = false;
                }
            }
        }

        IEnumerator Fire(Vector3 direction)
        {
            yield return new WaitForSeconds(1);
            while (isFire)
            {
                Quaternion rotate = Quaternion.LookRotation(direction);
                GameObject newBullet = BulletPool.Instance.Get();
                newBullet.transform.position = bulletSpawnPoint.position;
                newBullet.transform.rotation = rotate;
                BulletController bulletController = newBullet.GetComponent<BulletController>();
                bulletController.bulletType = BulletType.Enemy;
                bulletController._rigidbody.velocity = direction * bulletController.startSpeed;

                yield return new WaitForSeconds(fireInterval);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, checkRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * 10);

            // Gizmos.color = Color.green;
            // Gizmos.DrawRay(bulletSpawnPoint.position,
            //     pc.eyeViewTrans.position - bulletSpawnPoint.position);
        }
    }
}