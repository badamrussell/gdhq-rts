using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/new TowerType")]
    public class TowerConfig : ScriptableObject
    {
        public EnemyType towerType;
        public int warBucksCost;
        public GameObject prefab;
    }
}
