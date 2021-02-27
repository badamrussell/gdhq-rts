using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Units
{

    public class TowerEmptyPlot : MonoBehaviour
    {

        public static event Action<bool> onMouseNearTowerPlotEvent;
        public static event Action onPlaceTowerEvent;

        enum TowerLocationMode
        {
            available,
            selected,
            occupied,
        }

        [SerializeField] private TowerLocationMode _towerLocationMode = TowerLocationMode.available;
        [SerializeField] private GameObject _selectionFx;
        [SerializeField] private GameObject _platform;

        private bool _buildModeEnabled = false;

        private bool IsOccupied {
            get
            {
                return _towerLocationMode == TowerLocationMode.occupied;
            }
        }

        void Start()
        {
            _selectionFx.SetActive(false);
        }

        private void OnEnable()
        {
            BuildTowerManager.onStartBuildMode += HandleStartBuildMode;
            BuildTowerManager.onExitBuildMode += HandleExitBuildMode;
        }

        private void OnDisable()
        {
            BuildTowerManager.onStartBuildMode -= HandleStartBuildMode;
            BuildTowerManager.onExitBuildMode -= HandleExitBuildMode;
        }

        private void HandleStartBuildMode(GameObject go)
        {
            if (IsOccupied) { return; }
            _buildModeEnabled = true;

            _selectionFx.SetActive(true);
        }

        private void HandleExitBuildMode()
        {
            _buildModeEnabled = false;

            _selectionFx.SetActive(false);
            if (_towerLocationMode == TowerLocationMode.selected) {
                _towerLocationMode = TowerLocationMode.available;
            }
        }

        private void OnMouseEnter()
        {
            if (!_buildModeEnabled || IsOccupied) { return; }
            if (onMouseNearTowerPlotEvent == null) { return; }

            onMouseNearTowerPlotEvent(true);
            _towerLocationMode = TowerLocationMode.selected;
        }

        private void OnMouseExit()
        {
            if (!_buildModeEnabled || IsOccupied) { return; }
            if (onMouseNearTowerPlotEvent == null) { return; }

            onMouseNearTowerPlotEvent(false);
            _towerLocationMode = TowerLocationMode.available;
        }

        private void OnMouseDown()
        {
            if (!_buildModeEnabled || IsOccupied) { return; }
            if (onPlaceTowerEvent == null) { return; }

            _towerLocationMode = TowerLocationMode.occupied;
            _platform.SetActive(false);

            onPlaceTowerEvent();
            onMouseNearTowerPlotEvent(false);
        }

    }
}
