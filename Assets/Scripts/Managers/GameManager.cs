using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;
using UnityEngine.SceneManagement;
    
namespace GameDevHQITP.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
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

        [SerializeField] private bool _isWavesComplete;
        [SerializeField] private int _enemyCount = 0;
        
        
        private void Start()
        {
            _healthGoodLimit = Mathf.RoundToInt(_startPlayerHealth * _healthGoodPercent);
            _healthOkayLimit =  Mathf.RoundToInt(_startPlayerHealth * _healthOkayPercent);
            
            _isWavesComplete = false;
            
            _warbucksTotal = _startWarBucks;
            UIManager.Instance.UpdateWarBucks(_warbucksTotal);
            
            HandlePlayerHealthChange(_startPlayerHealth);
            
            UIManager.Instance.OnPlay();
            SpawnManager.Instance.Reset();
        }
        
        private void OnEnable()
        {
            Enemy.OnEnemyStartDeath += HandleEnemyKilled;
            SpawnManager.OnEnemySpawned += HandleEnemySpawned;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyStartDeath -= HandleEnemyKilled;
            SpawnManager.OnEnemySpawned -= HandleEnemySpawned;
        }
        
        private void HandleEnemyKilled(EnemyConfig enemyConfig, GameObject go, bool killedByPlayer)
        {
            if (killedByPlayer)
            {
                _warbucksTotal += enemyConfig.warBucks;
                UIManager.Instance.UpdateWarBucks(_warbucksTotal);
            }
        
            _enemyCount--;
        
            if (_enemyCount <= 0 && _isWavesComplete)
            {
                OnGameOver();
            }
        }

        public void HandlePlayerHealthChange(int value)
        {
            _playerHealth += value;
            
            if (value > _healthGoodLimit)
            {
                UIManager.Instance.UpdatePlayerHealth(_playerHealth, Color.white, "Good");
            } else if (value > _healthOkayLimit)
            {
                UIManager.Instance.UpdatePlayerHealth(_playerHealth, _okayColor, "Okay");
            } else if (value > 0)
            {
                UIManager.Instance.UpdatePlayerHealth(_playerHealth, _poorColor, "Low");
            }
            else
            {
                UIManager.Instance.UpdatePlayerHealth(_playerHealth, _poorColor, "Dead");
                OnGameOver();
            }
        }
        
        public void OnRestart()
        {
            SceneManager.LoadScene("Start_Level");
        }
        
        public void OnGameOver()
        {
            if (_playerHealth > 0)
            {
                UIManager.Instance.ShowMenu("LEVEL COMPLETE");
            }
            else
            {
                UIManager.Instance.ShowMenu("LEVEL FAILED");
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
        
        public void HandleEnemySpawned(EnemyType enemyType)
        {
            _enemyCount++;
        }
        
        public bool MakePurchase(int value)
        {
            if (value <= _warbucksTotal)
            {
                _warbucksTotal -= value;
                UIManager.Instance.UpdateWarBucks(_warbucksTotal);
                return true;
            }

            return false;
        }
        
        public bool CanAfford(int amount)
        {
            return amount <= _warbucksTotal;
        }
    }
}

