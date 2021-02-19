using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] private WaveConfig[] _waves;

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        foreach (WaveConfig wc in _waves)
        {
            yield return new WaitForSeconds(wc.initialDelay);
            foreach (int i in System.Linq.Enumerable.Range(0, wc.enemyPatternCount))
            {
                foreach (EnemyType enemyType in wc.enemyPattern)
                {

                    SpawnManager.Instance.SpawnEnemy(enemyType);
                    yield return new WaitForSeconds(wc.spawnRate);
                }
            }
        }

        Debug.Log("All Waves Complete");
    }
}
