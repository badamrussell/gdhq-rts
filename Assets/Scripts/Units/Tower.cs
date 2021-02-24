using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.Units
{

    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject _attackRadius;
        [SerializeField] private Color _successColor;
        [SerializeField] private Color _failColor;

        void Start()
        {
            ShowRedAttack(this.gameObject);
        }

        private void OnEnable()
        {
            TowerLocation.onNearTowerPlacement += ShowGreenAttack;
            TowerLocation.onExitTowerPlacement += ShowRedAttack;
            TowerLocation.onReadyForBattle += ReadyForBattle;
        }

        private void OnDisable()
        {
            TowerLocation.onNearTowerPlacement -= ShowGreenAttack;
            TowerLocation.onExitTowerPlacement -= ShowRedAttack;
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

        private void ReadyForBattle(GameObject go)
        {
            if (go != gameObject) { return; }

            TowerLocation.onNearTowerPlacement -= ShowGreenAttack;
            TowerLocation.onExitTowerPlacement -= ShowRedAttack;

            Debug.Log("TOWER READY FOR BATTLE!");

            _attackRadius.gameObject.SetActive(false);
        }
  
    }
}
