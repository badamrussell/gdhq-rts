using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevHQITP.Managers;
using GameDevHQITP.Widgets;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;
using UnityEngine.PlayerLoop;
using VSCodeEditor;


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

    public class Enemy : FollowTarget, IAttackable
    {
        public static event Action<EnemyConfig, GameObject> OnEnemyEndDeath;
        public static event Action<EnemyConfig, GameObject> OnEnemyStartDeath;
        public static Action<GameObject, int> OnTakeDamage;

        [SerializeField] private EnemyConfig enemyConfig;

        [SerializeField] private int _warFund;
        [SerializeField] private ProgressMeter _progressMeter;
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private GameObject _targetArea;

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _attackTarget;
        [SerializeField] private GameObject _parentRender;
        
        [SerializeField] private Renderer[] _dissolveRenderers;
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

        private void Start()
        {
            _animator.SetBool("IsAlive", true);

            _dissolveRenderers = _parentRender.GetComponentsInChildren<Renderer>();
        }

        private void Update()
        {
            base.Update();
            if (_isAttacking)
            {
                float fDamage = _baseDamagePerSec * Time.deltaTime;
                OnTakeDamage(_attackTarget, Mathf.RoundToInt((fDamage)));
            }
        }
        
        public void InitiateActive()
        {
            _animator.SetBool("IsAlive", true);
            _animator.SetBool("IsWalking", false);

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
            _animator.SetBool("IsAlive", false);
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

                _animator.SetBool("IsWalking", true);
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

        private IEnumerator OnFadeDeath()
        {
            float dissolveAmount = 0f;
            float timeInc = 0.01f;
            float totalTime = 4f;
            float increment = 1f * timeInc / totalTime;
            while (dissolveAmount < 1f)
            {
                for (int i = 0; i < _dissolveRenderers.Length; i++)
                {
                    _dissolveRenderers[i].material.SetFloat("_fillAmount", dissolveAmount);
                }
                
                dissolveAmount += increment;
                yield return new WaitForEndOfFrame();
            }
        }
        
        public IEnumerator OnDeath()
        {
            if (OnEnemyStartDeath != null)
            {
                OnEnemyStartDeath(enemyConfig, gameObject);
            }

            yield return OnFadeDeath();
            
            _animator.WriteDefaultValues();
            gameObject.SetActive(false);

            if (OnEnemyEndDeath != null)
            {
                // OnEnemyEndDeath(enemyConfig, gameObject);
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
        
        protected override void StartAttack(GameObject target)
        {
            Debug.Log($"START ATTACK {_damageRate} | {_baseDamagePerSec}");
            _animator.SetBool("IsAttacking", true);
            _attackTarget = target;
        }

        protected override void StopAttack()
        {
            Debug.Log("STOP ATTACK");
            _animator.SetBool("IsAttacking", false);
            _attackTarget = null;
        }
  
    }

}
