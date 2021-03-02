using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameDevHQITP.Units;
using GameDevHQITP.Widgets;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Managers
{
    // Manages between the different tower states; construction, battle, upgrade, etc

    public class TowerController : MonoBehaviour
    {
        [SerializeField] private TowerBattleReady _battleReadyTower;
        [SerializeField] private TowerConstruction _constructionTower;
        [SerializeField] private ProgressMeter _progressMeter;

        [SerializeField] private float _constructionSpeed = 10f;
        [SerializeField] private bool _isBuilt;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _warBucksCost;
        [SerializeField] private float _currentHealth;
        [SerializeField] private IntVariable _warBucks;

        void Start()
        {
            _battleReadyTower.gameObject.SetActive(_isBuilt);
            _constructionTower.gameObject.SetActive(!_isBuilt);

            if (_isBuilt)
            {
                _currentHealth = _maxHealth;
                _progressMeter.progressValue = 1f;

                _battleReadyTower.gameObject.SetActive(_isBuilt);
                _constructionTower.gameObject.SetActive(!_isBuilt);
            } else
            {
                StartCoroutine(UpdateBuildProgress());
            }
        }

        private IEnumerator UpdateBuildProgress()
        {
            _currentHealth = 0.1f;
            _warBucks.value -= _warBucksCost;

            while (_currentHealth < _maxHealth)
            {
                _currentHealth += Time.deltaTime * _constructionSpeed;
                _progressMeter.progressValue = _currentHealth / _maxHealth;

                if (_currentHealth < 0f)
                {
                    break;   
                }
                yield return null;
            }

            if (_currentHealth > 0) {
                _isBuilt = true;
                _currentHealth = _maxHealth;

                _battleReadyTower.gameObject.SetActive(_isBuilt);
                _constructionTower.gameObject.SetActive(!_isBuilt);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
