using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitPrefab
{
    public UnitType unitType;
    public GameObject prefab;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField]
    private WaveConfig[] _waves;

    [SerializeField]
    private UnitPrefab[] _unitPrefabs;

    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    
    [SerializeField] private GameObject _spawnContainer;
    
    private List<Enemy> _enemyPool = new List<Enemy>();


    void Start()
    {
        if (SpawnManager.Instance == null)
        {
            SpawnManager.Instance = this;
            Time.timeScale = 8f;
        }

        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        foreach (WaveConfig wc in _waves)
        {
            yield return new WaitForSeconds(wc.initialDelay);

            for (int i = 0; i < wc.loops; i++)
            {
                foreach (UnitType unitType in wc._enemyPattern)
                {
                    SpawnUnit(unitType);
                    yield return new WaitForSeconds(wc.spawnRate);
                }
            }

        }

        Debug.Log("Wave Complete");
    }

    private GameObject SpawnNewEnemy(GameObject enemyPrefab)
    {
        GameObject goEnemy = Instantiate(enemyPrefab, _startPoint.position, Quaternion.identity);
        goEnemy.transform.parent = _spawnContainer.transform;
        Enemy enemy = goEnemy.GetComponent<Enemy>();
        enemy.SetDestination(_endPoint.position);
        return goEnemy;
    }


    private GameObject GetEnemyPrefab(UnitType unitType)
    {
        foreach(UnitPrefab u in _unitPrefabs) {
            if (u.unitType == unitType)
            {
                return u.prefab;
            }
        }

        return _unitPrefabs[0].prefab;
    }
    
    private GameObject SpawnUnit(UnitType unitType)
    {
        int i = _enemyPool.FindIndex(e => e.unitType == unitType);

        if (i > -1)
        {
            GameObject go = _enemyPool[i].gameObject;
            _enemyPool.RemoveAt(i);
            go.transform.position = _startPoint.position;
            go.SetActive(true);
            return go;
        }
        
        return SpawnNewEnemy(GetEnemyPrefab(unitType));
    }

    public void EnemyDestroyed(GameObject goEnemy)
    {
        
    }

    public void EnemyReachedTarget(GameObject goEnemy)
    {
        goEnemy.SetActive(false);
        _enemyPool.Add(goEnemy.GetComponent<Enemy>());
    }

    
}
