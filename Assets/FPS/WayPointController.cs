using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vanks.FPS;

namespace Vanks
{
    public class WayPointController : MonoBehaviour
    {
        public GameObject[] enemies;
        private NavMeshAgent[] _enemiesNavMeshAgent;
        private EnemyController[] _enemiesController;
        public Transform[] wayPoints;
        public int[] currentWayPointIndex;
        public float speed = 1;

        private void Start()
        {
            wayPoints = gameObject.GetComponentsInChildren<Transform>();
            currentWayPointIndex = new int[enemies.Length];
            _enemiesNavMeshAgent = new NavMeshAgent[enemies.Length];
            _enemiesController = new EnemyController[enemies.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                _enemiesNavMeshAgent[i] = enemies[i].GetComponent<NavMeshAgent>();
                _enemiesController[i] = enemies[i].GetComponent<EnemyController>();
            }

            SetNextDestination();
        }

        private void Update()
        {
            SetNextDestination();
        }

        private void SetNextDestination()
        {
            if (wayPoints.Length <= 1) return;

            for (int i = 0; i < enemies.Length; i++)
            {
                if (!_enemiesController[i].isCheckPlayer)
                {
                    if (!_enemiesNavMeshAgent[i].pathPending && _enemiesNavMeshAgent[i].remainingDistance < 0.5f)
                    {
                        currentWayPointIndex[i] = (currentWayPointIndex[i] + 1) % wayPoints.Length;
                        _enemiesNavMeshAgent[i].SetDestination(wayPoints[currentWayPointIndex[i]].position);
                    }
                }
                else
                {
                    _enemiesNavMeshAgent[i].SetDestination(_enemiesController[i].player.transform.position);
                }
            }
        }
    }
}