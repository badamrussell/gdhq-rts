using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Units
{

    enum TowerLocationMode
    {
        available,
        selected,
        occupied,
    }

    public class TowerLocation : MonoBehaviour
    {

        public static event Action<GameObject> onNearTowerPlacement;
        public static event Action<GameObject> onExitTowerPlacement;
        public static event Action<GameObject> onReadyForBattle;

        [SerializeField] private TowerLocationMode _towerLocationMode;
        [SerializeField] private GameObject _selectionFx;
        [SerializeField] private GameObject _platform;

        private GameObject _goTower;

        void Start()
        {
            SetMode(TowerLocationMode.available);
            _platform.SetActive(false);
        }

        private void OnEnable()
        {
            BuildTowerManager.onBuildTower += BuildTower;
            BuildTowerManager.onCancelBuild += CancelBuild;
        }

        private void OnDisable()
        {
            BuildTowerManager.onBuildTower -= BuildTower;
            BuildTowerManager.onCancelBuild -= CancelBuild;
        }

        private void BuildTower(GameObject goTower)
        {
            if (_towerLocationMode != TowerLocationMode.selected) { return; }
            _goTower = goTower;

            SetMode(TowerLocationMode.occupied);
            goTower.transform.parent = this.transform;
            goTower.transform.localPosition = Vector3.zero;

            onReadyForBattle(goTower);
        }

        private void CancelBuild()
        {
            if (_towerLocationMode != TowerLocationMode.selected) { return; }
            SetMode(TowerLocationMode.available);
        }

        private void SetMode(TowerLocationMode mode)
        {
            _towerLocationMode = mode;

            switch (mode)
            {
                case TowerLocationMode.selected:
                    _selectionFx.SetActive(true);
                    break;
                case TowerLocationMode.available:
                case TowerLocationMode.occupied:
                default:
                    _selectionFx.SetActive(false);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_towerLocationMode == TowerLocationMode.occupied) { return; }

            if (other.tag == "TowerToBuild")
            {
                if (onNearTowerPlacement != null)
                {
                    float dist = Vector3.Distance(transform.position, other.transform.position);
                    onNearTowerPlacement(other.gameObject);
                    SetMode(TowerLocationMode.selected);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_towerLocationMode == TowerLocationMode.occupied) { return; }

            if (other.tag == "TowerToBuild")
            {
                if (onExitTowerPlacement != null)
                {
                    onExitTowerPlacement(other.gameObject);
                    SetMode(TowerLocationMode.available);
                }
            }
        }

    }
}
