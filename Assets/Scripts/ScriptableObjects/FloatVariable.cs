using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "floatVariable.asset", menuName = "ScriptableObjects/new FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        public float value;
    }
}
