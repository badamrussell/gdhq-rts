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

    private void Awake()
    {
        _instance = this;
        Time.timeScale = 10f;

        _prefabLookup = new Dictionary<EnemyType, GameObject>();
        foreach(GameObject go in _enemyPrefabs)
        {
            Enemy e = go.GetComponent<Enemy>();
            _prefabLookup.Add(e.EnemyType, go);
        }
    }

    private GameObject SpawnNewEnemy(EnemyType enemyType)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyType);
        GameObject goEnemy = Instantiate(enemyPrefab, _startPoint.position, Quaternion.identity);

        return goEnemy;
    }

    private GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        return _prefabLookup[enemyType] ? _prefabLookup[enemyType] : _enemyPrefabs[0];
    }

    public GameObject SpawnEnemy(EnemyType enemyType)
    {
        GameObject go = PoolManager.Instance.Get(enemyType.ToString());
        if (go != null)
        {
            go.transform.position = _startPoint.position;
            go.SetActive(true);
        } else
        {
            go = SpawnNewEnemy(enemyType);
            go.transform.parent = _spawnContainer.transform;
            go.transform.position = _startPoint.position;
        }

        return go;
    }

    public void EnemyDestroyed(GameObject goEnemy)
    {
        
    }

    public void EnemyReachedGoal(GameObject goEnemy)
    {
        EnemyType enemyType = goEnemy.GetComponent<Enemy>().EnemyType;
        string keyName = enemyType.ToString();
        goEnemy.SetActive(false);
        PoolManager.Instance.Add(keyName, goEnemy);
    }
    
}
