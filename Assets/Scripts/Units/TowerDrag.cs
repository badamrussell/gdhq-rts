using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.Units
{

    public class TowerDrag : MonoBehaviour
    {
        [SerializeField] private GameObject _attackRadius;
        [SerializeField] private Color _successColor;
        [SerializeField] private Color _failColor;

        void Start()
        {
            ShowRedAttack(gameObject);
        }

        private void OnEnable()
        {
            TowerEmptyPlot.onNearTowerPlacement += ShowGreenAttack;
            TowerEmptyPlot.onExitTowerPlacement += ShowRedAttack;
        }

        private void OnDisable()
        {
            TowerEmptyPlot.onNearTowerPlacement -= ShowGreenAttack;
            TowerEmptyPlot.onExitTowerPlacement -= ShowRedAttack;
        }

        private void ShowGreenAttack(GameObject go)
        {
            _attackRadius.GetComponent<Renderer>().material.color = _successColor;
        }

        private void ShowRedAttack(GameObject go)
        {
            if (go != gameObject) { return; }

            _attackRadius.GetComponent<Renderer>().material.color = _failColor;
        }

    }
}
