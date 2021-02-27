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
            IsNearPlot(false);
        }

        private void OnEnable()
        {
            TowerEmptyPlot.onMouseNearTowerPlotEvent += IsNearPlot;
        }

        private void OnDisable()
        {
            TowerEmptyPlot.onMouseNearTowerPlotEvent -= IsNearPlot;
        }

        private void IsNearPlot(bool isNear)
        {
            if (isNear)
            {
                _attackRadius.GetComponent<Renderer>().material.color = _successColor;
            }
            else
            {
                _attackRadius.GetComponent<Renderer>().material.color = _failColor;
            }
        }

    }
}
