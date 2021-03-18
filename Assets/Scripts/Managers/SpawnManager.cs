using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        
        public static event Action<EnemyType> OnEnemySpawned;
        
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

        private float CalculateTimeForWave(int index)
        {
            WaveConfig wc = _waves[index];
            if (wc == null)
            {
                return 0f;
            }

            Debug.Log($"{wc.initialDelay} + {wc.enemyPattern.Length} * {wc.enemyPatternCount} * {wc.spawnRate}");
            return wc.initialDelay + (wc.enemyPattern.Length * wc.enemyPatternCount * wc.spawnRate);
        }
        
        private IEnumerator StartWaves()
        {
            int waveCount = 0;
            float startTime = Time.time;

            for (var wci = 0 ; wci < _waves.Length; wci++)
            {
                WaveConfig wc = _waves[wci];
                float waveTime = CalculateTimeForWave(wci);
                waveCount++;
                
                UIManager.Instance.WaveStarted(waveCount, _waves.Length, waveTime);
                yield return new WaitForSeconds(wc.initialDelay);
                foreach (int i in System.Linq.Enumerable.Range(0, wc.enemyPatternCount))
                {
                    foreach (EnemyType enemyType in wc.enemyPattern)
                    {

                        SpawnEnemy(enemyType);
                        if (OnEnemySpawned != null)
                        {
                            OnEnemySpawned(enemyType);
                        }
                        
                        yield return new WaitForSeconds(wc.spawnRate);
                    }
                }
                
                yield return new WaitForSeconds(1f);
            }

            GameManager.Instance.OnWavesCompleted();
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

