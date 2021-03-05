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

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        public EnemyType EnemyType
        {
            get
            {
                return _enemyType;
            }
        }

        public void InitiateActive()
        {
            _animator.SetBool("isAlive", true);

            _health = _maxHealth;
            _progressMeter.progressValue = 1f;

            _health = _maxHealth;
            _progressMeter.progressValue = 1f;

            gameObject.SetActive(true);
        }

        public void InitiateDeath()
        {
            _animator.SetBool("isAlive", false);
            _navMeshAgent.isStopped = true;
            StartCoroutine(OnDeath());
        }

        private void OnEnable()
        {
            PlayerHome.onReachedPlayerBase += ReachedPlayerBase;
            TowerBattleReady.OnTakeDamage += TakeDamage;

            InitiateActive();

            Vector3 goal = SpawnManager.Instance.GoalPosition;
            _navMeshAgent.SetDestination(goal);
            _navMeshAgent.isStopped = false;
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
            _animator.WriteDefaultValues();
            gameObject.SetActive(false);

            if (OnEnemyDestroyed != null)
            {
                OnEnemyDestroyed(_enemyType, gameObject);
            }
        }

        public void TakeDamage(GameObject target, int damage)
        {
            if (target != _targetArea) { return; }
            _health -= damage;

            _progressMeter.progressValue = (float)_health / _maxHealth;

            if (_health <= 0)
            {
                InitiateDeath();
            }
        }
    }

}
