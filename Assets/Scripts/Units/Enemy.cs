using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevHQITP.Managers;
using GameDevHQITP.Units;


namespace GameDevHQITP.Units
{
    public enum EnemyType
    {
        Heavy,
        Infantry,
        TowerGatlingGun,
        TowerMissileLauncher,
        None
    }

    public class Enemy : MonoBehaviour
    {
        public static event Action<EnemyType, GameObject> onEnemyDestroyed;

        [SerializeField] private int _health;
        [SerializeField] private int _warFund;
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private EnemyType _enemyType;
        public EnemyType EnemyType
        {
            get
            {
                return _enemyType;
            }
        }


        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Enemy enemy = GetComponent<Enemy>();

            if (!enemy)
            {
                Debug.Log("EnemyNavigation is not attached to Enemy");
            }
            else
            {
                _enemyType = enemy.EnemyType;
            }
            if (!_navMeshAgent)
            {
                Debug.LogError("NavMesh Agent is NULL");
            }
        }

        private void OnEnable()
        {
            PlayerHome.onReachedPlayerBase += ReachedPlayerBase;

            Vector3 goal = SpawnManager.Instance.GoalPosition;
            _navMeshAgent.SetDestination(goal);
        }

        private void OnDisable()
        {
            PlayerHome.onReachedPlayerBase -= ReachedPlayerBase;
        }

        public void ReachedPlayerBase(GameObject go)
        {
            if (gameObject != go) { return; }

            if (onEnemyDestroyed != null)
            {
                onEnemyDestroyed(_enemyType, gameObject);
            }
        }
    }

}