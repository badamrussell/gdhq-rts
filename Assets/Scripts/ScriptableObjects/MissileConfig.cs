using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "missileConfig.asset", menuName = "ScriptableObjects/new MissileConfig")]
    public class MissileConfig : ScriptableObject
    {
        public float missileSpeed;
        public float turningDamping;
        public float gravityOffset;
    }
}
