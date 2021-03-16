using System;
using System.Collections;
using System.Collections.Generic;
using GameDevHQITP.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevHQITP.Widgets
{
    public class ArmoryButton : MonoBehaviour
    {
        [SerializeField] private TowerConfig _towerConfig;
        [SerializeField] private Button _button;

        public bool IsAffordable(int amount)
        {
            if (_towerConfig.warBucksCost <= amount)
            {
                _button.interactable = true;
                return true;
            }
            
            _button.interactable = false;
            return false;
        }
    }
}
