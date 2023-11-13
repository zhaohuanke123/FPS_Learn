using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Pool;

namespace Vanks.FPS
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int bulletPoolSize = 20;
        [SerializeField] private int bulletPoolMaxSize = 100;
        public Queue<GameObject> pool;

        public static BulletPool Instance { get; private set; }

        void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;

            pool = new Queue<GameObject>();
        }

        void Start()
        {
            for (int i = 0; i < bulletPoolSize; i++)
            {
                var bullet = Instantiate(bulletPrefab, transform);
                bullet.SetActive(false);
                pool.Enqueue(bullet);
            }
        }

        public GameObject Get()
        {
            if (pool.Count > 0)
            {
                var bullet = pool.Dequeue();
                bullet.SetActive(true);
                return bullet;
            }
            else
            {
                var bullet = Instantiate(bulletPrefab, transform);
                bullet.SetActive(true);
                return bullet;
            }
        }

        public void Release(GameObject bullet)
        {
            bullet.SetActive(false);
            if (pool.Count < bulletPoolMaxSize)
                pool.Enqueue(bullet);
            else Destroy(bullet);
        }

        public void Release(GameObject bullet, float delay)
        {
            StartCoroutine(ReleaseDelay(bullet, delay));
        }

        IEnumerator ReleaseDelay(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }
}