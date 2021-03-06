using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Managers
{
    [Serializable]
    struct PoolPrefabLookup
    {
        public EnemyType enemyType;
        public GameObject prefab;
    }

    public class PoolManager : MonoSingleton<PoolManager>
    {

        [SerializeField] private PoolPrefabLookup[] _prefabs;
        [SerializeField] private int _poolSeedAmount = 10;
        [SerializeField] private GameObject _poolContainer;

        private Dictionary<EnemyType, Queue<GameObject>> _pool = new Dictionary<EnemyType, Queue<GameObject>>();


        private void Start()
        {
            SeedStartingPool();
        }

        private void OnEnable()
        {
            Enemy.OnEnemyEndDeath += Remove;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyEndDeath -= Remove;
        }

        private void SeedStartingPool()
        {
            foreach(PoolPrefabLookup p in _prefabs)
            {
                _pool[p.enemyType] = new Queue<GameObject>();

                foreach (int i in Enumerable.Range(0, _poolSeedAmount))
                {
                    CreateEnemy(p.enemyType);
                }
            }
        }

        private GameObject CreateEnemy(EnemyType enemyType)
        {
            GameObject prefab = _prefabs.First(p => p.enemyType == enemyType).prefab;
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.parent = _poolContainer.transform;
            go.SetActive(false);
            _pool[enemyType].Enqueue(go);

            return go;
        }

        public GameObject Get(EnemyType enemyType)
        {
            try
            {
                return _pool[enemyType].Dequeue();
            } catch
            {
                CreateEnemy(enemyType);
                return _pool[enemyType].Dequeue();
            }
        }

        public void Remove(EnemyConfig enemyConfig, GameObject go, bool killedByPLayer)
        {
            if (!_pool.ContainsKey(enemyConfig.enemyType))
            {
                Queue<GameObject> goQue = new Queue<GameObject>();
                _pool.Add(enemyConfig.enemyType, goQue);
            }

            go.SetActive(false);

            _pool[enemyConfig.enemyType].Enqueue(go);
        }

    }
}
