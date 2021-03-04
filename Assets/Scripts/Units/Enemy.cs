using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevHQITP.Managers;
using GameDevHQITP.Widgets;
using GameDevHQITP.Units;


namespace GameDevHQITP.Units
{
    public enum EnemyType
    {
        Heavy,
        Infantry,
        None
    }

    public class Enemy : MonoBehaviour
    {
        public static event Action<EnemyType, GameObject> OnEnemyDestroyed;

        [SerializeField] private int _warFund;
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private ProgressMeter _progressMeter;
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private GameObject _targetArea;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

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
            _animator = GetComponent<Animator>();
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
            TowerBattleReady.OnTakeDamage += TakeDamage;

            Vector3 goal = SpawnManager.Instance.GoalPosition;
            _navMeshAgent.SetDestination(goal);

            _health = _maxHealth;
            _progressMeter.progressValue = 1f;

            _navMeshAgent.isStopped = false;
            _animator.SetBool("isAlive", true);
        }

        private void OnDisable()
        {
            PlayerHome.onReachedPlayerBase -= ReachedPlayerBase;
            TowerBattleReady.OnTakeDamage -= TakeDamage;
        }

        public void ReachedPlayerBase(GameObject go)
        {
            if (gameObject != go) { return; }

            if (OnEnemyDestroyed != null)
            {
                OnEnemyDestroyed(_enemyType, gameObject);
            }
        }

        public IEnumerator OnDeath()
        {
            yield return new WaitForSeconds(5f);
            OnEnemyDestroyed(_enemyType, gameObject);
        }

        public void TakeDamage(GameObject target, int damage)
        {
            if (target != _targetArea) { return; }
            _health -= damage;

            _progressMeter.progressValue = (float)_health / _maxHealth;

            if (_health <= 0)
            {
                _navMeshAgent.isStopped = true;
                _animator.SetBool("isAlive", false);
                // look into Write Defaults for 
                StartCoroutine(OnDeath());

                OnEnemyDestroyed(_enemyType, _targetArea);
            }
        }
    }

}
