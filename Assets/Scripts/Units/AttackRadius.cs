using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Units
{
    // Detects "Attackable" objects within zone
    // removes target if event::onTargetAreaDestroyed is called
    // returns best target GetTargetPosition
    public class AttackRadius : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _nearbyTargets;
        [SerializeField] private string[] _attackableTags;

        private GameObject _targetedNeighbor;
        private GameObject _targetedGO;

        private void Start()
        {
            _nearbyTargets = new List<GameObject>();
        }

        private void OnEnable()
        {
            Enemy.OnEnemyStartDeath += RemoveNeighbor;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyStartDeath -= RemoveNeighbor;
        }

        private void RemoveNeighbor(EnemyConfig enemyConfig, GameObject enemyGO)
        {
            if (_targetedNeighbor == enemyGO)
            {
                _targetedGO = null;
                _targetedNeighbor = null;
            }

            _nearbyTargets.RemoveAll(e => e == enemyGO);
        }

        public GameObject GetLockedOnUnit()
        {
            return _targetedNeighbor;
        }

        public GameObject GetLockedOnTarget()
        {
            return _targetedGO;
        }

        public bool GetTarget(out GameObject targetPos)
        {
            if (_targetedGO != null)
            {
                targetPos = _targetedGO;
                return true;
            }

            if (_nearbyTargets.Count > 0)
            {
                // TO_DO: Instead of taking first, is there a better algorithm?
                // if enemy leaves attackRadius, select closest enemy & lock on
                _targetedNeighbor = _nearbyTargets[0];
                Debug.Log(_targetedNeighbor);

                IAttackable attackableTest = _targetedNeighbor.GetComponent<IAttackable>();

                if (attackableTest != null)
                {
                    _targetedGO = attackableTest.GetAttackTarget();
                } else
                {
                    _targetedGO = _targetedNeighbor.gameObject;
                }

                targetPos = _targetedGO;

                return true;
            }

            targetPos = null;
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Array.Exists(_attackableTags, a => a == other.tag)) { return; }
            _nearbyTargets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!Array.Exists(_attackableTags, a => a == other.tag)) { return; }

            RemoveNeighbor(null, other.gameObject);
        }
    }
}
