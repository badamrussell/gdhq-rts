using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace GameDevHQITP.Managers
{
    [System.Serializable]
    public struct AvailableTower
    {
        public GameObject dragging;
        public GameObject tower;
    }

    public class BuildTowerManager : MonoBehaviour
    {
        public static event Action<GameObject> onStartBuildMode;
        public static event Action onExitBuildMode;
        public static event Action<GameObject> onStartTowerConstruction;

        public delegate void onSelectStartBuild(int index);

        [SerializeField] private AvailableTower[] _availableTowerPrefabs;
        [SerializeField] private GameObject _goTowerToBuild;
        [SerializeField] private GameObject _towerPlotContainer;
        [SerializeField] private GameObject _hideTowerContainer;
        [SerializeField] private int _selectedTowerIndex = 0;
        [SerializeField] private float _dragHeightOffset = 1f;

        private GameObject[] _towerInstances;

        private void Start()
        {
            _towerInstances = new GameObject[_availableTowerPrefabs.Length];
            for(int i = 0; i < _availableTowerPrefabs.Length; i++)
            {
                _towerInstances[i] = Instantiate(_availableTowerPrefabs[i].dragging);
                _towerInstances[i].transform.parent = _hideTowerContainer.transform;
            }
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && !_goTowerToBuild)
            {
                EnableBuildMode();
            }

            if (_goTowerToBuild != null)
            {
                Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(rayOrigin, out hitInfo))
                {
                    _goTowerToBuild.transform.position = new Vector3(hitInfo.point.x, _dragHeightOffset, hitInfo.point.z);
                }

                if (Input.GetMouseButton(0))
                {
                    onStartTowerConstruction(_availableTowerPrefabs[_selectedTowerIndex].tower);
                    DisableBuildMode();
                } else if (Input.GetMouseButton(1))
                {
                    DisableBuildMode();
                }
            }
        }

        private void EnableBuildMode()
        {
            _goTowerToBuild = _towerInstances[_selectedTowerIndex];
            _goTowerToBuild.transform.parent = _towerPlotContainer.transform;
            onStartBuildMode(_goTowerToBuild);
        }

        private void DisableBuildMode()
        {
            if (!_goTowerToBuild) { return; }

            _goTowerToBuild.transform.parent = _hideTowerContainer.transform;
            _goTowerToBuild = null;
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
