using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Widgets;
using GameDevHQITP.Utility;
using System;

namespace GameDevHQITP.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {

        public enum EnumArmoryMenuModes
        {
            purchase,
            upgradeMissile,
            upgradeGatling,
            dismantle
        }


        public static event Action<GameObject> OnSelectedTower;
        [SerializeField] List<GameObject> _selectedTowers = new List<GameObject>();

        [SerializeField] private GameObject _purchaseMenu;
        [SerializeField] private GameObject _upgradeMissileMenu;
        [SerializeField] private GameObject _upgradeGatlingMenu;
        [SerializeField] private GameObject _dismantleMenu;

        [SerializeField] private EnumArmoryMenuModes _armoryMode;
        [SerializeField] private SelectTower _selectTower;


        private void Start()
        {
            _armoryMode = EnumArmoryMenuModes.purchase;
            SetArmoryMenu();
        }

        private void OnEnable()
        {
            MenuChoice.OnAccept += DoMenuAction;
            MenuChoice.OnCancel += EnablePurchaseMode;
        }

        private void OnDisable()
        {
            MenuChoice.OnAccept -= DoMenuAction;
            MenuChoice.OnCancel -= EnablePurchaseMode;
        }

        private void DoMenuAction()
        {
            switch (_armoryMode)
            {
                case EnumArmoryMenuModes.dismantle:
                    Debug.Log("UPGRADE DISMANTLE");
                    break;
                case EnumArmoryMenuModes.upgradeGatling:
                    Debug.Log("UPGRADE GATLING");
                    break;
                case EnumArmoryMenuModes.upgradeMissile:
                    Debug.Log("UPGRADE MISSILE");
                    break;
                case EnumArmoryMenuModes.purchase:
                default:
                    break;
            }

            EnablePurchaseMode();
            SetArmoryMenu();
        }

        private void SetArmoryMenu()
        {
            switch (_armoryMode)
            {
                case EnumArmoryMenuModes.dismantle:
                    _purchaseMenu.SetActive(false);
                    _upgradeMissileMenu.SetActive(false);
                    _upgradeGatlingMenu.SetActive(false);
                    _dismantleMenu.SetActive(true);
                    break;
                case EnumArmoryMenuModes.upgradeGatling:
                    _purchaseMenu.SetActive(false);
                    _upgradeMissileMenu.SetActive(false);
                    _upgradeGatlingMenu.SetActive(true);
                    _dismantleMenu.SetActive(false);
                    break;
                case EnumArmoryMenuModes.upgradeMissile:
                    _purchaseMenu.SetActive(false);
                    _upgradeMissileMenu.SetActive(true);
                    _upgradeGatlingMenu.SetActive(false);
                    _dismantleMenu.SetActive(false);
                    break;
                case EnumArmoryMenuModes.purchase:
                default:
                    _purchaseMenu.SetActive(true);
                    _upgradeMissileMenu.SetActive(false);
                    _upgradeGatlingMenu.SetActive(false);
                    _dismantleMenu.SetActive(false);
                    break;
            }
        }

        public void OnSelectTower(TowerConfig towerConfig, GameObject go)
        {
            _selectedTowers.Clear();
            _selectedTowers.Add(go);
            //OnSelectedTower(go);

            _selectTower.Init(towerConfig, go);

            //switch (towerConfig.towerType)
            //{
            //    case Units.TowerType.TowerGatlingGun:
            //        _armoryMode = EnumArmoryMenuModes.upgradeGatling;
            //        break;
            //    case Units.TowerType.TowerMissileLauncher:
            //        _armoryMode = EnumArmoryMenuModes.upgradeMissile;
            //        break;
            //    default:
            //        _armoryMode = EnumArmoryMenuModes.purchase;
            //        break;
            //}

            //SetArmoryMenu();
        }

        private void EnablePurchaseMode()
        {

            _armoryMode = EnumArmoryMenuModes.purchase;
        }

        private void EnableUpgradeMode(TowerConfig towerConfig, GameObject go)
        {

            _armoryMode = EnumArmoryMenuModes.purchase;
        }

        private void EnableDismantleMode(TowerConfig towerConfig)
        {

            _armoryMode = EnumArmoryMenuModes.dismantle;
        }
    }
    
}
