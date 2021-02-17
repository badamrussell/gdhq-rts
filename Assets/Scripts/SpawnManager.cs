using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SpawnManager is NULL");
            }

            return _instance;
        }
    }

    [SerializeField] private WaveConfig[] _waves;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private GameObject _spawnContainer;
    [SerializeField] private GameObject[] _enemyPrefabs;

    public Vector3 GoalPosition
    {
        get
        {
            return _endPoint.position;
        }
    }

    private Dictionary<EnemyType, GameObject> _prefabLookup;
    private List<Enemy> _enemyPool = new List<Enemy>();

    private void Awake()
    {
        _instance = this;
        Time.timeScale = 8f;

        _prefabLookup = new Dictionary<EnemyType, GameObject>();
        foreach(GameObject go in _enemyPrefabs)
        {
            Enemy e = go.GetComponent<Enemy>();
            _prefabLookup.Add(e.EnemyType, go);
        }
    }

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        foreach (WaveConfig wc in _waves)
        {
            yield return new WaitForSeconds(wc.initialDelay);

            foreach(int i in System.Linq.Enumerable.Range(0, wc.enemyPatternCount))
            {
                foreach (EnemyType enemyType in wc.enemyPattern)
                {
                    SpawnEnemy(enemyType);
                    yield return new WaitForSeconds(wc.spawnRate);
                }
            }

        }

        Debug.Log("All Waves Complete");
    }

    private GameObject SpawnNewEnemy(GameObject enemyPrefab)
    {
        GameObject goEnemy = Instantiate(enemyPrefab, _startPoint.position, Quaternion.identity);
        goEnemy.transform.parent = _spawnContainer.transform;
        return goEnemy;
    }


    private GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        return _prefabLookup[enemyType] ? _prefabLookup[enemyType] : _enemyPrefabs[0];
    }
    
    private GameObject SpawnEnemy(EnemyType enemyType)
    {
        int i = _enemyPool.FindIndex(e => e.EnemyType == enemyType);

        if (i > -1)
        {
            GameObject go = _enemyPool[i].gameObject;
            _enemyPool.RemoveAt(i);
            go.transform.position = _startPoint.position;
            go.SetActive(true);
            return go;
        }
        
        return SpawnNewEnemy(GetEnemyPrefab(enemyType));
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
