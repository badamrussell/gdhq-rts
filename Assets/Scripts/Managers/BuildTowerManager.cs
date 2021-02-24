using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameDevHQITP.Managers
{

    public class BuildTowerManager : MonoBehaviour
    {

        public static event Action<GameObject> onBuildTower;
        public static event Action onCancelBuild;

        [SerializeField] private GameObject _towerPrefab;

        [SerializeField] private GameObject _goTowerToBuild;


        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && !_goTowerToBuild)
            {
                _goTowerToBuild = Instantiate(_towerPrefab);
                _goTowerToBuild.tag = "TowerToBuild";
            }


            if (_goTowerToBuild != null)
            {
                Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(rayOrigin, out hitInfo))
                {
                    _goTowerToBuild.transform.position = new Vector3(hitInfo.point.x, 1f, hitInfo.point.z);
                }

                if (Input.GetMouseButton(0))
                {
                    // BUGFIX - need to handle scenario with left-click and not in legal tower placement area
                    _goTowerToBuild.tag = "Tower";
                    onBuildTower(_goTowerToBuild);
                    _goTowerToBuild = null;
                } else if (Input.GetMouseButton(1))
                {
                    onCancelBuild();
                    Destroy(_goTowerToBuild);
                    _goTowerToBuild = null;
                }
            }


        }
    }
}
