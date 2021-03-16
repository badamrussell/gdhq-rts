using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Widgets;
using GameDevHQITP.Utility;
using System;
using GameDevHQITP.Units;
using UnityEngine.UI;

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

        [SerializeField] private EnumArmoryMenuModes _armoryMode;
        [SerializeField] private SelectTower _selectTower;

        [SerializeField] private int _playerHealth = 100;
        [SerializeField] private int _startPlayerHealth = 100;
        [SerializeField] private float _healthGoodPercent = 0.6f;
        [SerializeField] private float _healthOkayPercent = 0.2f;
        private int _healthGoodLimit;
        private int _healthOkayLimit;

        private int _warbucksTotal = 0;
        [SerializeField] private int _startWarBucks = 1000;
        
        [SerializeField] private Color _okayColor;
        [SerializeField] private Color _poorColor;
        
        [SerializeField] private Text _lifeText;
        [SerializeField] private Text _waveText;
        [SerializeField] private Text _versionText;
        [SerializeField] private Text _warBucksText;
        [SerializeField] private Text _healthStatusText;
        [SerializeField] private Text _levelStatusText;
        [SerializeField] private Text _timerText;

        [SerializeField] private GameObject _levelStatus;
        [SerializeField] private GameObject _playbackPauseEnabled;
        [SerializeField] private GameObject _playbackPlayEnabled;
        [SerializeField] private GameObject _playbackFFEnabled;

        [SerializeField] private List<Image> _colorizedOverlays;
        [SerializeField] private Image[] _colorOverlays;
        [SerializeField] private ArmoryButton[] _armoryButtons;

        [SerializeField] private bool _isWavesComplete;
        [SerializeField] private int _enemyCount = 0;
        
        private void Start()
        {
            _armoryMode = EnumArmoryMenuModes.purchase;

            _healthGoodLimit = Mathf.RoundToInt(_startPlayerHealth * _healthGoodPercent);
            _healthOkayLimit =  Mathf.RoundToInt(_startPlayerHealth * _healthOkayPercent);

            _colorizedOverlays = new List<Image>();
            foreach (Transform child in transform)
            {               
                Image image = child.gameObject.GetComponent<Image>();
                if (image != null)
                {
                    _colorizedOverlays.Add(image);
                }
            }
            OnRestart();
        }

        private IEnumerator StartTimer()
        {
            int minutes = 0;
            int seconds = -1;
            while (true)
            {
                seconds++;
                if (seconds == 60)
                {
                    minutes++;
                    seconds = 0;
                }
                
                string sec = seconds.ToString().PadLeft(2, '0' );
                _timerText.text = $"{minutes}:{sec}";
                yield return new WaitForSeconds(1f);
            }
        }
        
        private void OnEnable()
        {
            BuildTowerManager.onNewWarBucksTotal += UpdateWarBucks;
            Enemy.OnEnemyStartDeath += HandleEnemyKilled;
        }

        private void OnDisable()
        {
            BuildTowerManager.onNewWarBucksTotal -= UpdateWarBucks;
            Enemy.OnEnemyStartDeath -= HandleEnemyKilled;
        }

        public void OnSelectTower(TowerConfig towerConfig, GameObject go)
        {
            _selectedTowers.Clear();
            _selectedTowers.Add(go);
            _selectTower.Init(towerConfig, go);
        }

        private void EnableUpgradeMode(TowerConfig towerConfig, GameObject go)
        {

            _armoryMode = EnumArmoryMenuModes.purchase;
        }

        private void EnableDismantleMode(TowerConfig towerConfig)
        {

            _armoryMode = EnumArmoryMenuModes.dismantle;
        }

        public void UpdatePlayerHealth(int damage)
        {
            _playerHealth -= damage;
            _lifeText.text = _playerHealth.ToString();
            
            if (_playerHealth > _healthGoodLimit)
            {
                _healthStatusText.text = "Good";
                ChangeOverlayColor(Color.white);
            } else if (_playerHealth > _healthOkayLimit)
            {
                _healthStatusText.text = "Okay";
                ChangeOverlayColor(_okayColor);
            } else if (_playerHealth > 0)
            {
                _healthStatusText.text = "Low";
                ChangeOverlayColor(_poorColor);
            }
            else
            {
                ChangeOverlayColor(_poorColor);
                OnGameOver();
            }
        }

        private void ChangeOverlayColor(Color color)
        {
            foreach (Image image in _colorOverlays)
            {
                image.color = color;
            }
        }
        
        public void UpdateWave(int value, int maxValue)
        {
            _waveText.text = $"{value}/{maxValue}";
        }

        public bool MakePurchase(int value)
        {
            if (value <= _warbucksTotal)
            {
                UpdateWarBucks(-value);
                return true;
            }

            return false;
        }
        
        public void UpdateWarBucks(int value)
        {
            _warbucksTotal += value;
            _warBucksText.text = _warbucksTotal.ToString();

            foreach (ArmoryButton btn in _armoryButtons)
            {
                btn.IsAffordable(_warbucksTotal);
            }
        }
        
        public void OnRestart()
        {
            _isWavesComplete = false;
            
            _warbucksTotal = _startWarBucks;
            UpdateWarBucks(0);
            
            _playerHealth = _startPlayerHealth;
            UpdatePlayerHealth(0);
            
            StopCoroutine("StartTimer");
            StartCoroutine("StartTimer");
        
            OnPlay();
            SpawnManager.Instance.Reset();
        }
        
        public void OnPause()
        {
            if (Time.timeScale == 0f)
            {
                OnPlay();
                return;
            }
            Time.timeScale = 0f;
            _playbackPauseEnabled.SetActive(true);
            _playbackPlayEnabled.SetActive(false);
            _playbackFFEnabled.SetActive(false);
            _levelStatus.SetActive(true);
            _levelStatusText.text = "PAUSED";
        }
        public void OnPlay()
        {
            Time.timeScale = 1f;
            _playbackPauseEnabled.SetActive(false);
            _playbackPlayEnabled.SetActive(true);
            _playbackFFEnabled.SetActive(false);
            _levelStatus.SetActive(false);
        }
        public void OnFastForward()
        {
            Time.timeScale = 20f;
            _playbackPauseEnabled.SetActive(false);
            _playbackPlayEnabled.SetActive(false);
            _playbackFFEnabled.SetActive(true);
            _levelStatus.SetActive(false);
        }

        public void OnGameOver()
        {
            if (_playerHealth > 0)
            {
                _levelStatusText.text = "LEVEL COMPLETE";
            }
            else
            {
                _levelStatusText.text = "LEVEL FAILED";
            }
            
            Time.timeScale = 1f;
            _playbackPauseEnabled.SetActive(false);
            _playbackPlayEnabled.SetActive(false);
            _playbackFFEnabled.SetActive(false);
            _levelStatus.SetActive(true);
        }
        
        private void HandleEnemyKilled(EnemyConfig enemyConfig, GameObject go, bool killedByPlayer)
        {
            if (killedByPlayer)
            {
                UpdateWarBucks(enemyConfig.warBucks);
            }

            _enemyCount--;

            if (_enemyCount <= 0 && _isWavesComplete)
            {
                OnGameOver();
            }
        }

        public void OnWavesCompleted()
        {
            _isWavesComplete = true;
            if (_enemyCount <= 0)
            {
                OnGameOver();
            }
        }

        public void AddEnemy()
        {
            _enemyCount++;
        }

        public bool CanAfford(int amount)
        {
            return amount <= _warbucksTotal;
        }
    }
    
}
