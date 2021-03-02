using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Widgets;

namespace GameDevHQITP.Units
{
    // this is an area that can be targeted for attack
    // has its own health and set a progress bar
    public class TargetArea : MonoBehaviour
    {
        public static event Action<GameObject> onTargetAreaDestroyed;

        [SerializeField] private Enemy _enemy;
        [SerializeField] private ProgressMeter _progressMeter;
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;

        private void OnEnable()
        {
            _health = _maxHealth;
            _progressMeter.progressValue = 1f;
            AttackRadius.onTakeDamage += TakeDamage;
        }

        private void OnDisable()
        {
            AttackRadius.onTakeDamage -= TakeDamage;
        }

        public void TakeDamage(GameObject target, int damage)
        {
            if (target != gameObject) { return; }

            _health -= damage;

            // TO_DO: should probably have the Enemy class do this
            _progressMeter.progressValue = (float)_health / _maxHealth;

            if (_health <= 0)
            {
                // TO_DO: This seems odd?
                // need to let enemy know this was destroyed
                // so could be an event, but only a specific enemy should react
                _enemy.OnTargetAreaDestroyed();

                onTargetAreaDestroyed(gameObject);
            }
        }
    }
}
