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

    private List<GameObject> _enemyPool = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTime += Time.deltaTime;

        if (_spawnTime >= _spawnRate)
        {
            SpawnEnemy(GetRandomEnemyType());
            _spawnTime = 0f;
        }
    }

    private GameObject GetRandomEnemyType()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
    }

    private void GetPooledOrNewEnemy()
    {
        
    }
    
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject goEnemy = Instantiate(enemyPrefab, _startPoint.position, Quaternion.identity);
        goEnemy.transform.parent = _spawnContainer.transform;
        Enemy enemy = goEnemy.GetComponent<Enemy>();
        enemy.SetDestination(_endPoint.position);
    }

    public void EnemyDestroyed(GameObject goEnemy)
    {
        
    }

    public void EnemyReachedTarget(GameObject goEnemy)
    {
        goEnemy.SetActive(false);
        _enemyPool.Add(goEnemy);
    }
}
