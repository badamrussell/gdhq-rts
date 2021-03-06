using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Widgets;
using UnityEngine.UI;
using TMPro;

namespace GameDevHQITP.Managers
{
    public class SelectionManager : MonoSingleton<SelectionManager>
    {

        public static event Action<GameObject> OnUpgradeTower;
        public static event Action<GameObject> OnDismantleTower;

        [SerializeField] List<GameObject> _selectedTowers = new List<GameObject>();

        [SerializeField] private GameObject _containerGO;
        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _upgradeGatlingItem;
        [SerializeField] private GameObject _upgradeMissileItem;
        [SerializeField] private Button _upgradeButton;

        [SerializeField] private TowerConfig _activeTowerConfig;

        [SerializeField] private TextMeshProUGUI _dismantlePrice;
        [SerializeField] private TextMeshProUGUI _purchasePrice;


        private void Start()
        {
            _containerGO.SetActive(false);
        }

        private void OnEnable()
        {
            TowerController.OnSelectedTower += OnSelectTower;
        }

        private void OnDisable()
        {
            TowerController.OnSelectedTower -= OnSelectTower;
        }

        public void OnSelectTower(TowerConfig towerConfig, GameObject go)
        {
            if (_selectedTowers.Contains(go))
            {
                _selectedTowers.Clear();
                OnHide();
                return;
            }

            _activeTowerConfig = towerConfig;

            if (towerConfig.towerType == TowerType.TowerGatlingGun)
            {
                _upgradeGatlingItem.SetActive(true);
                _upgradeMissileItem.SetActive(false);
            } else if (towerConfig.towerType == TowerType.TowerMissileLauncher)
            {
                _upgradeGatlingItem.SetActive(false);
                _upgradeMissileItem.SetActive(true);
            }

            _dismantlePrice.text = $"$ {towerConfig.warBucksSell}";
            _purchasePrice.text = $"- $ {towerConfig.warBucksCost}";

            _selectedTowers.Clear();
            _selectedTowers.Add(go);

            _containerGO.transform.position = go.transform.position;

            OnShow();
        }

        public void OnShow()
        {
            _containerGO.SetActive(true);

            _animator.SetBool("IsVisible", true);
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
            
            _upgradeButton.interactable = UIManager.Instance.CanAfford(_activeTowerConfig.upgradeCost) && _activeTowerConfig.upgradePrefab != null;
        }

        public void OnHide()
        {

            _containerGO.SetActive(false);

            _animator.SetBool("IsVisible", false);
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
            _selectedTowers.Clear();
        }

        public void OnSell()
        {
            _animator.SetBool("IsDismantle", true);
            _animator.SetBool("IsUpgrade", false);
        }

        public void OnUpgrade()
        {

            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", true);
        }

        public void OnConfirmSell()
        {
            OnDismantleTower(_selectedTowers[0]);
            UIManager.Instance.UpdateWarBucks(_activeTowerConfig.warBucksSell);
            OnHide();
        }

        public void OnConfirmUpgrade()
        {
            if (UIManager.Instance.MakePurchase(_activeTowerConfig.warBucksCost))
            {
                OnUpgradeTower(_selectedTowers[0]);
            }
            OnHide();
        }

        public void OnCancel()
        {
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
        }
    }
}
