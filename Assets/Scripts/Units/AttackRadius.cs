using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameDevHQITP.Units
{
    // Detects "Attackable" objects within zone
    // removes target if event::onTargetAreaDestroyed is called
    // returns best target GetTargetPosition
    public class AttackRadius : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _nearbyTargets = new List<GameObject>();

        private GameObject _targetedNeighbor;
        private GameObject _targetedGO;

        private void OnEnable()
        {
            Enemy.OnEnemyStartDeath += RemoveNeighbor;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyStartDeath -= RemoveNeighbor;
        }

        private void RemoveNeighbor(EnemyType enemyType, GameObject enemyGO)
        {
            if (_targetedNeighbor == enemyGO)
            {
                _targetedGO = null;
                _targetedNeighbor = null;
            }

            _nearbyTargets.RemoveAll(e => e == enemyGO);
        }

        public GameObject GetLockedOnTarget()
        {
            return _targetedGO;
        }

        public bool GetTargetPosition(out GameObject targetPos)
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
                targetPos = _nearbyTargets[0];

                IAttackable attackableTest = _targetedNeighbor.GetComponent<IAttackable>();

                if (attackableTest != null)
                {
                    _targetedGO = attackableTest.GetAttackTarget();
                } else
                {
                    _targetedGO = _targetedNeighbor.gameObject;
                }

                return true;
            }

            targetPos = null;
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") { return; }

            _nearbyTargets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Enemy") { return; }

            RemoveNeighbor(EnemyType.None, other.gameObject);
        }
    }
}
