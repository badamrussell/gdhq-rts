using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Units
{
    /*
     * What is weird here is saving the reference for _towerBeingDragged.
     * It is used so it only reacts to physics collisions with that object
     */
    public class TowerEmptyPlot : MonoBehaviour
    {

        public static event Action<GameObject> onNearTowerPlacement;
        public static event Action<GameObject> onExitTowerPlacement;

        enum TowerLocationMode
        {
            available,
            selected,
            occupied,
        }

        [SerializeField] private TowerLocationMode _towerLocationMode;
        [SerializeField] private GameObject _selectionFx;
        [SerializeField] private GameObject _platform;

        private GameObject _occupyingTower;
        private GameObject _towerBeingDragged;

        void Start()
        {
            _towerLocationMode = _occupyingTower ? TowerLocationMode.occupied : TowerLocationMode.available;
            _selectionFx.SetActive(false);
        }

        private void OnEnable()
        {
            BuildTowerManager.onStartBuildMode += HandleStartBuildMode;
            BuildTowerManager.onExitBuildMode += HandleExitBuildMode;
            BuildTowerManager.onStartTowerConstruction += BuildTower;
        }

        private void OnDisable()
        {
            BuildTowerManager.onStartBuildMode -= HandleStartBuildMode;
            BuildTowerManager.onExitBuildMode -= HandleExitBuildMode;
            BuildTowerManager.onStartTowerConstruction -= BuildTower;
        }

        private void HandleStartBuildMode(GameObject go)
        {
            if (_towerLocationMode == TowerLocationMode.occupied) { return; }

            _selectionFx.SetActive(true);
            _towerBeingDragged = go;
        }

        private void HandleExitBuildMode()
        {
            _selectionFx.SetActive(false);
            _towerBeingDragged = null;
            if (_towerLocationMode == TowerLocationMode.selected) {
                _towerLocationMode = TowerLocationMode.available;
            }
        }

        private void BuildTower(GameObject go)
        {
            if (_towerLocationMode != TowerLocationMode.selected) { return; }
            _towerLocationMode = TowerLocationMode.occupied;
            _towerBeingDragged = null;
            _platform.SetActive(false);

            _occupyingTower = Instantiate(go);
            _occupyingTower.transform.parent = transform;
            _occupyingTower.transform.localPosition = Vector3.zero;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_towerLocationMode == TowerLocationMode.occupied) { return; }
            if (other.gameObject != _towerBeingDragged) { return; }
            if (onNearTowerPlacement == null) { return; }

            onNearTowerPlacement(other.gameObject);
            _towerLocationMode = TowerLocationMode.selected;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_towerLocationMode == TowerLocationMode.occupied) { return; }
            if (other.gameObject != _towerBeingDragged) { return; }
            if (onExitTowerPlacement == null) { return; }

            onExitTowerPlacement(other.gameObject);
            _towerLocationMode = TowerLocationMode.available;
        }

    }
}
