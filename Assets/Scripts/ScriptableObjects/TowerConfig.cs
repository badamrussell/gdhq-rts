using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/new TowerType")]
    public class TowerConfig : ScriptableObject
    {
        public TowerType towerType;
        public int warBucksCost;
        public GameObject prefab;
        public float attackRadius;
        public int level;
        public int maxHealth;
        public float constructionTime;
        public int warBucksSell;
    }
}
