using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "boolVariable.asset", menuName = "ScriptableObjects/new BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        public bool value;
    }
}

