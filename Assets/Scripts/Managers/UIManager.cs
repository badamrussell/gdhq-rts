using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Widgets;
using GameDevHQITP.Utility;
using UnityEngine.UI;

namespace GameDevHQITP.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] List<GameObject> _selectedTowers = new List<GameObject>();

        [SerializeField] private SelectTower _selectTower;
        
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

        [SerializeField] private Image[] _colorOverlays;
        [SerializeField] private ArmoryButton[] _armoryButtons;

        private float _remainingTime;
 
        
        public void WaveStarted(int value, int maxValue, float secondsUntilNextWave)
        {
            _remainingTime = secondsUntilNextWave;
            _waveText.text = $"{value}/{maxValue}";
            
            StopCoroutine("CountdownForNextWave");
            StartCoroutine("CountdownForNextWave");
        }

        private IEnumerator CountdownForNextWave()
        {
            if (_remainingTime == 0f)
            {
                _timerText.text = "Final wave!";
                yield return null;
            }
            
            int timeRemaining = Mathf.RoundToInt(_remainingTime);
            int minutes = timeRemaining / 60;
            int seconds = timeRemaining - (minutes * 60);
            
            while (minutes + seconds >= 0)
            {
                string sec = seconds.ToString().PadLeft(2, '0' );
                _timerText.text = $"Next wave in: {minutes}:{sec}";
                
                seconds--;
                if (seconds == -1)
                {
                    minutes--;
                    seconds = 59;
                }
                
                yield return new WaitForSeconds(1f);
            }
        }

        public void OnSelectTower(TowerConfig towerConfig, GameObject go)
        {
            _selectedTowers.Clear();
            _selectedTowers.Add(go);
            _selectTower.Init(towerConfig, go);
        }

        public void UpdatePlayerHealth(int value, Color color, string description)
        {
            _lifeText.text = value.ToString();
            ChangeOverlayColor(color);
            _healthStatusText.text = description;
        }

        private void ChangeOverlayColor(Color color)
        {
            foreach (Image image in _colorOverlays)
            {
                image.color = color;
            }
        }
        
        public void UpdateWarBucks(int value)
        {
            _warBucksText.text = value.ToString();

            foreach (ArmoryButton btn in _armoryButtons)
            {
                btn.IsAffordable(value);
            }
        }
        
        public void OnPause()
        {
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
            
        public void ShowMenu(string description)
        {
            _levelStatusText.text = description;
            
            Time.timeScale = 1f;
            _playbackPauseEnabled.SetActive(false);
            _playbackPlayEnabled.SetActive(false);
            _playbackFFEnabled.SetActive(false);
            _levelStatus.SetActive(true);
        }
        
    }
    
}
