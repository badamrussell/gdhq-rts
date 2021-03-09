using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevHQITP.Managers;
using GameDevHQITP.Widgets;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;



namespace GameDevHQITP.Units
{
    public interface IDamageable<T>
    {
        public void Thing();
    }

    public enum EnemyType
    {
        Heavy,
        Infantry,
        None
    }

    public class Enemy : MonoBehaviour, IAttackable
    {
        public static event Action<EnemyConfig, GameObject> OnEnemyEndDeath;
        public static event Action<EnemyConfig, GameObject> OnEnemyStartDeath;

        [SerializeField] private EnemyConfig enemyConfig;

        [SerializeField] private int _warFund;
        [SerializeField] private ProgressMeter _progressMeter;
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private GameObject _targetArea;

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Collider _collider;

        public EnemyType EnemyType
        {
            get
            {
                return enemyConfig.enemyType;
            }
        }

        public GameObject GetAttackTarget()
        {
            return _targetArea;
        }

        public void InitiateActive()
        {
            _animator.SetBool("isAlive", true);
            _collider.enabled = true;

            _health = _maxHealth;
            _progressMeter.progressValue = 1f;

            _navMeshAgent.enabled = true;

            _health = _maxHealth;
            _progressMeter.progressValue = 1f;

            gameObject.SetActive(true);
        }

        public void InitiateDeath()
        {
            _animator.SetBool("isAlive", false);
            _collider.enabled = false;
            _navMeshAgent.enabled = false;
            _navMeshAgent.isStopped = true;
            StartCoroutine(OnDeath());
        }

        private void OnEnable()
        {
            PlayerHome.onReachedPlayerBase += ReachedPlayerBase;
            TowerBattleReady.OnTakeDamage += TakeDamage;

            InitiateActive();

            if (SpawnManager.Instance != null)
            {
                Vector3 goal = SpawnManager.Instance.GoalPosition;
                _navMeshAgent.SetDestination(goal);
                _navMeshAgent.isStopped = false;
            }
        }

        private void OnDisable()
        {
            PlayerHome.onReachedPlayerBase -= ReachedPlayerBase;
            TowerBattleReady.OnTakeDamage -= TakeDamage;
        }

        public void ReachedPlayerBase(GameObject go)
        {
            if (gameObject != go) { return; }

            if (OnEnemyEndDeath != null)
            {
                OnEnemyEndDeath(enemyConfig, gameObject);
            }
        }

        public IEnumerator OnDeath()
        {
            if (OnEnemyStartDeath != null)
            {
                OnEnemyStartDeath(enemyConfig, gameObject);
            }
            yield return new WaitForSeconds(5f);
            _animator.WriteDefaultValues();
            gameObject.SetActive(false);

            if (OnEnemyEndDeath != null)
            {
                OnEnemyEndDeath(enemyConfig, gameObject);
            }
        }

        public void TakeDamage(GameObject target, int damage)
        {
            if (target != _targetArea && target != gameObject) { return; }
            _health -= damage;

            _progressMeter.progressValue = (float)_health / _maxHealth;

            if (_health <= 0)
            {
                InitiateDeath();
            }
        }

        
    }

}
