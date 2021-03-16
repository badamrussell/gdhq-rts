using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missle_Launcher.Missle;
using GameDevHQITP.Units;

namespace GameDevHQITP.Units
{
    public class TowerMissileLauncher : TowerBattleReady
    {
        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private GameObject[] _misslePositionsLeft; //array to hold the rocket positions on the turret
        [SerializeField]
        private GameObject[] _misslePositionsRight; //array to hold the rocket positions on the turret


        [SerializeField]
        private float _missileXRotationOffset; //the two models have different values for these?

        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        private float _destroyTime = 10.0f; //how long till the rockets get cleaned up
        private bool _launched; //bool to check if we launched the rockets

        private GameObject _target;

        new void Update()
        {
            base.Update();
            //if (Input.GetKeyDown(KeyCode.Space) && _launched == false) //check for space key and if we launched the rockets
            if (_isAttacking && _launched == false)
            {
                _launched = true; //set the launch bool to true
                StartCoroutine("FireRocketsRoutine"); //start a coroutine that fires the rockets. 
            }
        }

        private void FireRocket(GameObject[] missilePos, int index)
        {
            GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

            rocket.transform.parent = missilePos[index].transform; //set the rockets parent to the missle launch position 

            rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero

            rocket.transform.localEulerAngles = new Vector3(_missileXRotationOffset, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction

            rocket.transform.parent = null; //set the rocket parent to null

            rocket.GetComponent<GameDevHQ.FileBase.Missle_Launcher.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime, _target); //assign missle properties 
            missilePos[index].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired
        }

        IEnumerator FireRocketsRoutine()
        {
            int maxIndex = Mathf.Max(_misslePositionsLeft.Length, _misslePositionsRight.Length);
            for (int i = 0; i < maxIndex; i++) //for loop to iterate through each missle position
            {
                if (i < _misslePositionsLeft.Length)
                {
                    FireRocket(_misslePositionsLeft, i);
                    yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
                }

                if (i < _misslePositionsRight.Length)
                {
                    FireRocket(_misslePositionsRight, i);
                    yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
                }
            }

            for (int i = 0; i < maxIndex; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                if (i < _misslePositionsLeft.Length)
                {
                    _misslePositionsLeft[i].SetActive(true); //enable fake rocket to show ready to fire
                }

                if (i < _misslePositionsRight.Length)
                {
                    _misslePositionsRight[i].SetActive(true); //enable fake rocket to show ready to fire
                }
            }

            _launched = false; //set launch bool to false
        }

        protected override void StartAttack(GameObject target)
        {
            _target = target;
        }

        protected override void StopAttack()
        {
            _target = null;
            StopCoroutine("FireRocketsRoutine");
            _launched = false;
        }
    }
}

