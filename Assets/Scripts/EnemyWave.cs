using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave
{
    public float spawnRate = 5f;
    public float initialDelay = 2f;
    
    public UnitType[] _enemyPattern;
    public int loops;

    public EnemyWave(float spawnRate, float initialDelay, int loops, UnitType[] pattern)
    {
        this.spawnRate = spawnRate;
        this.initialDelay = initialDelay;
        this.loops = loops;
        this._enemyPattern = pattern;
    }
}
