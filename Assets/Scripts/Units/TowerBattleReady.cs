using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.Units
{

    public class TowerBattleReady : MonoBehaviour
    {
        [SerializeField] private GameObject _attackRadius;

        private void Start()
        {
            _attackRadius.SetActive(false);
        }
    }
}
