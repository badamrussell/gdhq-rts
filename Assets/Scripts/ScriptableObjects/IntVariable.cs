using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.ScriptableObjects
{
    [CreateAssetMenu(fileName = "intVariable.asset", menuName = "ScriptableObjects/new IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public int value;
    }
}
