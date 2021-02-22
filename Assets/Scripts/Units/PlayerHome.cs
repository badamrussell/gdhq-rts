using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Units
{
    public class PlayerHome : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                EnemyType enemyType = other.gameObject.GetComponent<Enemy>().EnemyType;
                SpawnManager.Instance.EnemyReachedGoal(enemyType, other.gameObject);
            }
        }
    }
}
