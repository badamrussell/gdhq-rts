using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<EnemyWave> _enemyWaves = new List<EnemyWave>();

    private float _spawnTime;
    
    private EnemyWave _currentWave;

    private List<UnitType> _remainingEnemies = new List<UnitType>();

    public bool inProgress = true;
    public bool isDelayed = true;
    
    private int _waveIndex = 0;
    void Start()
    {
        _enemyWaves.Add(new EnemyWave(5f, 1f, 4, new UnitType[] { UnitType.Infantry }));
        _enemyWaves.Add(new EnemyWave(6f, 10f, 8, new UnitType[] { UnitType.Heavy }));
        _enemyWaves.Add(new EnemyWave(7f, 20f, 4, new UnitType[] { UnitType.Infantry, UnitType.Heavy }));
        _enemyWaves.Add(new EnemyWave(8f, 20f, 4, new UnitType[] { UnitType.Infantry, UnitType.Infantry, UnitType.Heavy }));
        _enemyWaves.Add(new EnemyWave(9f, 20f, 6, new UnitType[] { UnitType.Infantry, UnitType.Heavy, UnitType.Heavy }));
        LoadNextWave();
    }

    void Update()
    {
        _spawnTime += Time.deltaTime;
        // Debug.Log($"UPDATE: {_spawnTime} | {_currentWave.spawnRate} | {isDelayed} | {_currentWave.initialDelay}");
        if (isDelayed)
        {
            if (_spawnTime >= _currentWave.initialDelay)
            {
                isDelayed = false;
            }
        } else if (_spawnTime >= _currentWave.spawnRate && inProgress)
        {
            UnitType nextUnit = GetNextUnit();

            if (nextUnit == UnitType.None)
            {
                Debug.Log("Complete");
            }
            else
            {
                SpawnManager.Instance.SpawnUnit(nextUnit);
            }
            _spawnTime = 0f;
        }
        
    }

    private void LoadNextWave()
    {
        if (_enemyWaves.Count == 0)
        {
            inProgress = false;
            return;
        }

        isDelayed = true;
        _spawnTime = 0;//_currentWave.initialDelay;
        
        for (int i = 0; i < _enemyWaves[0].loops; i++)
        {
            foreach (UnitType unitType in _enemyWaves[0]._enemyPattern)
            {
                _remainingEnemies.Add(unitType);
            }
        }
        
        _currentWave = _enemyWaves[0];
        _enemyWaves.RemoveAt(0);
        _waveIndex++;
    }

    private UnitType GetNextUnit()
    {
        if (_remainingEnemies.Count == 0)
        {
            LoadNextWave();
            return UnitType.None;
        }

        // int total = _currentWave.loops * _currentWave._enemyPattern.Length;
        // Debug.Log($"Wave: {_waveIndex} | ({_currentWave.loops}, {_currentWave._enemyPattern.Length}) | Enemy: {total - _remainingEnemies.Count} of {total}");
        
        UnitType nextUnit = _remainingEnemies[0];
        _remainingEnemies.RemoveAt(0);
        return nextUnit;
    }
}
