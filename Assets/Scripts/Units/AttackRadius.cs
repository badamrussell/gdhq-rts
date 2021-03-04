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
        //public static event Action<GameObject, int> onTakeDamage;

        [SerializeField] private List<GameObject> _nearbyTargets = new List<GameObject>();
        [SerializeField] GameObject _targetedArea;

        private void OnEnable()
        {
            Enemy.OnEnemyDestroyed += RemoveTarget;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyDestroyed -= RemoveTarget;
        }

        private void RemoveTarget(EnemyType enemyType, GameObject target)
        {
            if (_targetedArea == target)
            {
                _targetedArea = null;
            }

            _nearbyTargets.RemoveAll(e => e == target);
        }

        public GameObject GetLockedOnTarget()
        {
            return _targetedArea;
        }

        public bool GetTargetPosition(out GameObject targetPos)
        {
            if (_targetedArea != null)
            {
                targetPos = _targetedArea;
                return true;
            }

            if (_nearbyTargets.Count > 0)
            {
                // TO_DO: Instead of taking first, is there a better algorithm?
                // if enemy leaves attackRadius, select closest enemy & lock on
                targetPos = _nearbyTargets[0];
                _targetedArea = _nearbyTargets[0];
                return true;
            }

            targetPos = null;
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Attackable") { return; }

            _nearbyTargets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Attackable") { return; }

            int index = _nearbyTargets.FindIndex(e => e == other.gameObject);
            if (index > -1)
            {
                if (_nearbyTargets[index] == _targetedArea)
                {
                    _targetedArea = null;
                }
                _nearbyTargets.RemoveAt(index);
            }
        }
    }
}
