using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameDevHQITP.Units
{
    public enum TowerType
    {
        TowerGatlingGun,
        TowerMissileLauncher,
    }

    public class TowerBattleReady : MonoBehaviour
    {

        public static event Action<GameObject, int> OnTakeDamage;

        [SerializeField] private AttackRadius _attackRadius;

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;

        [SerializeField] float _rotationSpeed = 2f;
        [SerializeField] GameObject _towerActionsGO;

        [SerializeField] float _horizontalAngleOffset = 0f;
        [SerializeField] float _verticalAngleOffset = 0f;
        [SerializeField] int _baseDamagePerSec = 20;

        [SerializeField] ITowerActions _towerActions;

        private int _damageRate;
        private bool _isFiring;

        private void Start()
        {
            _damageRate = Mathf.Max(_baseDamagePerSec / 10, 1);
            _towerActions = _towerActionsGO.GetComponent<ITowerActions>();
            _isFiring = false;
        }

        private IEnumerator SendDamage()
        {
            while (_isFiring)
            {
                yield return new WaitForSeconds(0.1f);
                OnTakeDamage(_attackRadius.GetLockedOnTarget(), _damageRate);
            }
        }

        private void Update()
        {
            GameObject targetGO;

            if (_attackRadius.GetTargetPosition(out targetGO))
            {
                RotateTurret(targetGO);
            } else if (_isFiring)
            {
                _isFiring = false;
                StopCoroutine("SendDamage");
                _towerActions.StopAttack();
            }
        }

        private void RotateTurret(GameObject targetGO)
        {
            Vector3 targetPos = targetGO.transform.position;
            // Rotate horizontally
            Vector3 horzTrans = _horizontalRotate.transform.position;
            Vector3 horzDiff = targetPos - horzTrans;
            Quaternion diffHorzRot = Quaternion.LookRotation(horzDiff);
            float rotHorzY = diffHorzRot.eulerAngles.y + _horizontalAngleOffset;
            Quaternion rotHorz = Quaternion.Euler(0, rotHorzY, 0);
            //_horizontalRotate.transform.rotation = rotHorz;
            //Debug.DrawLine(horzTrans, targetPos, Color.blue, 1f);

            // Rotate vertically
            Vector3 vertTrans = _verticalRotate.transform.position;
            Vector3 vertDiff = targetPos - vertTrans;
            Quaternion diffRot = Quaternion.LookRotation(vertDiff);
            float rotX = diffRot.eulerAngles.x + _verticalAngleOffset;
            Quaternion rotVert = Quaternion.Euler(rotX, rotHorzY, 0);
            //_verticalRotate.transform.rotation = rotVert;
            //Debug.DrawLine(vertTrans, targetPos, Color.green, 1f);

            _horizontalRotate.transform.rotation = Quaternion.Slerp(_horizontalRotate.transform.rotation, rotHorz, Time.deltaTime * _rotationSpeed);
            _verticalRotate.transform.rotation = Quaternion.Slerp(_verticalRotate.transform.rotation, rotVert, Time.deltaTime * _rotationSpeed);

            float rotDiff = Mathf.Abs(_horizontalRotate.transform.rotation.eulerAngles.y - rotHorzY);
            //Debug.Log($"{_horizontalRotate.transform.rotation.eulerAngles.y} > {rotHorzY} = {rotDiff}");
            if (rotDiff < 30f)
            {
                if (_isFiring)
                {
                    Debug.DrawLine(vertTrans, targetPos, Color.green, 0.1f);
                } else {

                    _towerActions.StartAttack(targetGO);
                    _isFiring = true;
                    StartCoroutine("SendDamage");
                }
            }
            else
            {
                Debug.DrawLine(vertTrans, targetPos, Color.yellow, 0.1f);

                _towerActions.StopAttack();
                _isFiring = false;
                StopCoroutine("SendDamage");
            }
        }

    }

}
