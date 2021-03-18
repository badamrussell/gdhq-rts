using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Units;

namespace GameDevHQITP.Managers
{
    [System.Serializable]
    public struct AvailableTower
    {
        public GameObject dragging;
        public TowerConfig towerConfig;
    }

    public class BuildTowerManager : MonoBehaviour
    {
        public static event Action<GameObject> onStartBuildMode;
        public static event Action onExitBuildMode;
        // public static event Action<int> onNewWarBucksTotal;

        [SerializeField] private AvailableTower[] _availableTowerPrefabs;
        [SerializeField] private GameObject _towerToBuildGO;
        [SerializeField] private GameObject _towerPlotContainer;
        [SerializeField] private GameObject _activeTowersContainer;
        [SerializeField] private GameObject _hideTowerContainer;

        private int _selectedTowerIndex = 0;
        private bool _buildModeEnabled = false;
        private bool _isNearTower = false;
        private GameObject[] _towerInstances;
        private Vector3 _selectedPlotPosition;

        private void Start()
        {
            _towerInstances = new GameObject[_availableTowerPrefabs.Length];
            for(int i = 0; i < _availableTowerPrefabs.Length; i++)
            {
                _towerInstances[i] = Instantiate(_availableTowerPrefabs[i].dragging);
                _towerInstances[i].transform.parent = _hideTowerContainer.transform;
            }
        }

        private void OnEnable()
        {
            TowerEmptyPlot.onMouseNearTowerPlotEvent += IsNearAvailableTowerPlot;
            TowerEmptyPlot.onPlaceTowerEvent += TryPlaceTower;
        }

        private void OnDisable()
        {
            TowerEmptyPlot.onMouseNearTowerPlotEvent -= IsNearAvailableTowerPlot;
            TowerEmptyPlot.onPlaceTowerEvent -= TryPlaceTower;
        }

        void Update()
        {
            DragTower();

            if (_buildModeEnabled && Input.GetMouseButton(1))
            { 
                DisableBuildMode();
            }
        }

        private void IsNearAvailableTowerPlot(bool isNear)
        {
            _isNearTower = isNear;
        }

        private void DragTower()
        {
            if (!_buildModeEnabled) { return; }

            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (_isNearTower)
                {
                    _selectedPlotPosition = hitInfo.transform.position;
                    _towerToBuildGO.transform.position = hitInfo.transform.position;
                }
                else
                {
                    _towerToBuildGO.transform.position = hitInfo.point;
                }
            }
        }

        private void TryPlaceTower()
        {
            TowerConfig config = _availableTowerPrefabs[_selectedTowerIndex].towerConfig;
            
            if (GameManager.Instance.MakePurchase(config.warBucksCost))
            {
                GameObject go = Instantiate(config.prefab, _selectedPlotPosition, Quaternion.identity);
                go.transform.parent = _activeTowersContainer.transform;
                DisableBuildMode();
            }

        }

        private void EnableBuildMode()
        {
            _buildModeEnabled = true;

            _towerToBuildGO = _towerInstances[_selectedTowerIndex];
            _towerToBuildGO.transform.parent = _towerPlotContainer.transform;
            onStartBuildMode(_towerToBuildGO);
        }

        private void DisableBuildMode()
        {
            _buildModeEnabled = false;
            if (_towerToBuildGO)
            {
                _towerToBuildGO.transform.parent = _hideTowerContainer.transform;
                _towerToBuildGO = null;
            }

            onExitBuildMode();
        }

        public void onSelectGatling()
        {
            _selectedTowerIndex = 0;
            DisableBuildMode();
            EnableBuildMode();
        }

        public void onSelectMissle()
        {
            _selectedTowerIndex = 1;
            DisableBuildMode();
            EnableBuildMode();
        }

    }
}
