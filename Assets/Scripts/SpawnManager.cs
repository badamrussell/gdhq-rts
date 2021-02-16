using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    
    [SerializeField]
    private GameObject[] _enemyPrefabs;

    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private float _spawnRate = 5f;
    private float _spawnTime = 5f;
    
    [SerializeField] private GameObject _spawnContainer;
    
    [SerializeField]
    private List<GameObject> _enemyPool = new List<GameObject>();

    public List<EnemyWave> waves = new List<EnemyWave>();
    private int _currentWaveIndex = 0;
    
    void Start()
    {

        if (SpawnManager.Instance == null)
        {
            SpawnManager.Instance = this;
            Time.timeScale = 8f;
        }
    }

    private UnitType GetRandomUnitType()
    {
        int randomInt = Random.Range(0, 10);

        if (randomInt < 5)
        {
            return UnitType.Infantry;
        }
        else
        {
            return UnitType.Heavy;
        }
    }
    
    private GameObject GetEnemyPrefab(UnitType unitType)
    {
        int index = 0;
        for (int i = 0; i < _enemyPrefabs.Length; i++) 
        {
            Enemy e = _enemyPrefabs[i].GetComponent<Enemy>();
            if (e.unitType == unitType)
            {
                index = i;
                break;
            }
        }
        
        return _enemyPrefabs[index];
    }
    
    private GameObject GetPooledOrNewEnemy(UnitType unitType)
    {
        int i = _enemyPool.FindIndex(eGO =>
        {
            Enemy e = eGO.GetComponent<Enemy>();
            return e.unitType == unitType;
        });

        if (i > -1)
        {
            GameObject go = _enemyPool[i];
            _enemyPool.RemoveAt(i);
            go.transform.position = _startPoint.position;
            go.SetActive(true);
            return go;
        }
        
        return SpawnNewEnemy(GetEnemyPrefab(unitType));
    }
    
    private GameObject SpawnNewEnemy(GameObject enemyPrefab)
    {
        GameObject goEnemy = Instantiate(enemyPrefab, _startPoint.position, Quaternion.identity);
        goEnemy.transform.parent = _spawnContainer.transform;
        Enemy enemy = goEnemy.GetComponent<Enemy>();
        enemy.SetDestination(_endPoint.position);
        return goEnemy;
    }

    public void EnemyDestroyed(GameObject goEnemy)
    {
        
    }

    public void EnemyReachedTarget(GameObject goEnemy)
    {
        goEnemy.SetActive(false);
        _enemyPool.Add(goEnemy);
    }

    public void SpawnUnit(UnitType unitType)
    {
        GetPooledOrNewEnemy(unitType);
    }
}
