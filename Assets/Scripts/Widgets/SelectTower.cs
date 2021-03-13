using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Utility;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Widgets
{
    public class SelectTower : MonoSingleton<SelectTower>
    {

        [SerializeField] private TowerActions _towerActions;
        [SerializeField] private SelectTower _selectTower;

        

        public void Init(TowerConfig towerConfig, GameObject go)
        {
            Debug.Log("INIT");
            gameObject.SetActive(true);
            _towerActions.OnShow();
        }

        //public void OnSelectTower(TowerConfig towerConfig, GameObject go)
        //{
        //    //_selectedTowers.Clear();
        //    //_selectedTowers.Add(go);
        //    //OnSelectedTower(go);

        //    //_selectTower.Init(towerConfig, go);

        //    //switch (towerConfig.towerType)
        //    //{
        //    //    case Units.TowerType.TowerGatlingGun:
        //    //        _armoryMode = EnumArmoryMenuModes.upgradeGatling;
        //    //        break;
        //    //    case Units.TowerType.TowerMissileLauncher:
        //    //        _armoryMode = EnumArmoryMenuModes.upgradeMissile;
        //    //        break;
        //    //    default:
        //    //        _armoryMode = EnumArmoryMenuModes.purchase;
        //    //        break;
        //    //}

        //    //SetArmoryMenu();
        //}
    }
}
