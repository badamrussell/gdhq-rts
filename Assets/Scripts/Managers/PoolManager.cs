using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;

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

        public void Remove(EnemyType enemyType, GameObject go)
        {
            if (!_pool.ContainsKey(enemyType))
            {
                Queue<GameObject> goQue = new Queue<GameObject>();
                _pool.Add(enemyType, goQue);
            }

            _pool[enemyType].Enqueue(go);
        }

    }
}
