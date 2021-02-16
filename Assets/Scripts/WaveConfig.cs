using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newWaveConfig.asset", menuName = "ScriptableObjects/new WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public float spawnRate = 5f;
    public float initialDelay = 2f;

    public UnitType[] _enemyPattern;
    public int loops;
}
