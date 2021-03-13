using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using GameDevHQITP.Units;
using GameDevHQITP.Widgets;
using GameDevHQITP.ScriptableObjects;
using GameDevHQITP.Managers;

namespace GameDevHQITP.Managers
{
    // Manages between the different tower states; construction, battle, upgrade, etc

    public class TowerController : MonoBehaviour
    {

        public static event Action<TowerConfig, GameObject> OnSelectedTower;

        [SerializeField] private TowerConfig _towerConfig;
        [SerializeField] private TowerBattleReady _battleReadyTower;
        [SerializeField] private TowerConstruction _constructionTower;
        [SerializeField] private ProgressMeter _progressMeter;
        //[SerializeField] private GameObject _selectedGO;

        [SerializeField] private float _constructionSpeed = 10f;
        [SerializeField] private ParticleSystem _constructionSmoke;
        [SerializeField] private ParticleSystem _dismantleCloud;

        [SerializeField] private bool _isBuilt;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _warBucksCost;
        [SerializeField] private float _currentHealth;
        [SerializeField] private IntVariable _warBucks;
        //[SerializeField] private TowerActions _towerActions;

        void Start()
        {
            //_battleReadyTower.gameObject.SetActive(_isBuilt);
            _constructionTower.gameObject.SetActive(!_isBuilt);
            //_selectedGO.SetActive(false);
            //_towerActions.OnHide();

            if (_isBuilt)
            {
                _currentHealth = _maxHealth;
                _progressMeter.progressValue = 1f;

                //_battleReadyTower.gameObject.SetActive(_isBuilt);
                _constructionTower.gameObject.SetActive(!_isBuilt);
                _battleReadyTower.Init();
            } else
            {
                StartCoroutine(UpdateBuildProgress());
                var ps = _constructionSmoke.main;
                _constructionSmoke.Stop();
                ps.duration = _towerConfig.constructionTime;
                _constructionSmoke.Play();
            }
        }

        private void OnEnable()
        {
            SelectionManager.OnUpgradeTower += OnUpgradeTower;
            SelectionManager.OnDismantleTower += OnDismantleTower;
        }

        private void OnDisable()
        {
            SelectionManager.OnUpgradeTower -= OnUpgradeTower;
            SelectionManager.OnDismantleTower -= OnDismantleTower;
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

                _battleReadyTower.Init();

                _constructionSmoke.Stop();
                //_battleReadyTower.gameObject.SetActive(_isBuilt);
                _constructionTower.gameObject.SetActive(!_isBuilt);
            } else {
                Destroy(gameObject);
            }
        }

        //private void SelectTower(GameObject go)
        //{
        //    if (go != gameObject)
        //    {
        //        //_selectedGO.SetActive(false);
        //        return;
        //    }

        //    Debug.Log("Unit selected");
        //    //_selectedGO.SetActive(true);

        //    //_towerActions.OnShow();
        //}

        private void OnMouseDown()
        {
            Debug.Log("Unit selected");
            if (OnSelectedTower != null)
            {
                OnSelectedTower(_towerConfig, gameObject);
            }
            //_selectedGO.SetActive(true);
        }

        public void OnUpgradeTower(GameObject go)
        {
            if (go != this.gameObject) { return; }

            Debug.Log("Show Upgrade");
            GameObject newTower = Instantiate(_towerConfig.upgradePrefab, transform.parent);
            newTower.transform.position = transform.position;
            Destroy(this.gameObject);
        }

        public void OnDismantleTower(GameObject go)
        {
            if (go != this.gameObject) { return; }

            _dismantleCloud.Play();

            _battleReadyTower.gameObject.SetActive(false);
            Debug.Log("Show Dismantle");
            Destroy(this.gameObject, 5f);
        }
    }
}
