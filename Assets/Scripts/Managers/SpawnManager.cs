using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {

        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        [SerializeField] private WaveConfig[] _waves;

        public Vector3 GoalPosition
        {
            get
            {
                return _endPoint.position;
            }
        }

        public void Reset()
        {
            StartCoroutine(StartWaves());
            
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Enemy enemy = child.GetComponent<Enemy>();
                if (child.activeSelf)
                {
                    PoolManager.Instance.Remove(enemy.enemyConfig, child, false);
                }
            }
        }
        
        private IEnumerator StartWaves()
        {
            int waveCount = 0;
            foreach (WaveConfig wc in _waves)
            {
                waveCount++;
                UIManager.Instance.UpdateWave(waveCount, _waves.Length);
                yield return new WaitForSeconds(wc.initialDelay);
                foreach (int i in System.Linq.Enumerable.Range(0, wc.enemyPatternCount))
                {
                    foreach (EnemyType enemyType in wc.enemyPattern)
                    {

                        SpawnEnemy(enemyType);
                        
                        UIManager.Instance.AddEnemy();
                        yield return new WaitForSeconds(wc.spawnRate);
                    }
                }
            }

            UIManager.Instance.OnWavesCompleted();
        }

        public GameObject SpawnEnemy(EnemyType enemyType)
        {
            GameObject go = PoolManager.Instance.Get(enemyType);
            go.SetActive(false);
            go.transform.position = _startPoint.position;
            go.SetActive(true);

            return go;
        }
    
    }
}

