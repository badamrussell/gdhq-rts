using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "enemyConfig.asset", menuName = "ScriptableObjects/new EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public EnemyType enemyType;
        public GameObject prefab;
        public int warBucks;
        public int defaultHealth;
        public float attackRadius;
        public int playerBaseDamage;
    }
}
