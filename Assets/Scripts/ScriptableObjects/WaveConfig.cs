using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;


namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName ="newWaveConfig.asset", menuName = "ScriptableObjects/new WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public float initialDelay = 2f;
        public float spawnRate = 5f;

        public int enemyPatternCount;
        public EnemyType[] enemyPattern;
    }
}
