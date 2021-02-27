using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameDevHQITP.Managers;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Widgets
{
    public class WarBucks : MonoBehaviour
    {
        [SerializeField] private IntVariable _warBucks;
        [SerializeField] private Text _text;

        void Start()
        {
            _text = GetComponent<Text>();
        }

        private void UpdateWarBucksDisplay(int newAmount)
        {
            _text.text = newAmount.ToString();
        }

        private void OnEnable()
        {
            BuildTowerManager.onNewWarBucksTotal += UpdateWarBucksDisplay;
        }

        private void OnDisable()
        {
            BuildTowerManager.onNewWarBucksTotal -= UpdateWarBucksDisplay;
        }
    }
}
