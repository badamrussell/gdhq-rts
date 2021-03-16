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

    abstract public class TowerBattleReady : MonoBehaviour, IAttackable
    {

        public static Action<GameObject, int> OnTakeDamage;

        [SerializeField] private AttackRadius _attackRadius;
        [SerializeField] protected float _attackRadiusAmount = 15f;
        [SerializeField] private GameObject _hitZone;

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;

        [SerializeField] float _rotationSpeed = 2f;

        [SerializeField] float _horizontalAngleOffset = 0f;
        [SerializeField] float _verticalAngleOffset = 0f;
        [SerializeField] int _baseDamagePerSec = 20;
        [SerializeField] private GameObject _targetGO;
        
        protected abstract void StopAttack();
        protected abstract void StartAttack(GameObject target);

        protected int _damageRate;
        protected bool _isAttacking;

        private bool _isReady = false;
     
        public void Init()
        {
            _isReady = true;
            _damageRate = Mathf.Max(_baseDamagePerSec / 10, 1);
            _isAttacking = false;
            _attackRadius.gameObject.transform.localScale = Vector3.one * _attackRadiusAmount;
        }

        protected void Update()
        {
            if (!_isReady) { return; }

            GameObject targetGO;
            //Debug.Log($"Update {_isAttacking}");

            if (_attackRadius.GetTarget(out targetGO))
            {
                if (targetGO != _targetGO)
                {
                    _isAttacking = false;
                    StopAttack();
                    _targetGO = targetGO;
                }
                RotateTurret(targetGO);
            } else if (_isAttacking)
            {
                _isAttacking = false;
                StopAttack();
            }
        }

        private void RotateTurret(GameObject targetGO)
        {
            Vector3 targetPos = targetGO.transform.position;
            //Debug.Log($"HAS TARGET {targetPos}");

            // Rotate horizontally
            Vector3 horzTrans = _horizontalRotate.transform.position;
            Vector3 horzDiff = targetPos - horzTrans;
            Quaternion diffHorzRot = Quaternion.LookRotation(horzDiff);
            float rotHorzY = diffHorzRot.eulerAngles.y + _horizontalAngleOffset;
            Quaternion rotHorz = Quaternion.Euler(0, rotHorzY, 0);
            Debug.DrawLine(horzTrans, targetPos, Color.blue, 0.1f);
            _horizontalRotate.transform.rotation = Quaternion.Slerp(_horizontalRotate.transform.rotation, rotHorz, Time.deltaTime * _rotationSpeed);

            Vector3 drawPos = horzTrans;

            if (_verticalRotate != null)
            {
                // Rotate vertically
                Vector3 vertTrans = _verticalRotate.transform.position;
                drawPos = vertTrans;
                Vector3 vertDiff = targetPos - vertTrans;
                Quaternion diffRot = Quaternion.LookRotation(vertDiff);
                float rotX = diffRot.eulerAngles.x + _verticalAngleOffset;
                Quaternion rotVert = Quaternion.Euler(rotX, rotHorzY, 0);
                //Debug.DrawLine(vertTrans, targetPos, Color.green, 1f);
                _verticalRotate.transform.rotation = Quaternion.Slerp(_verticalRotate.transform.rotation, rotVert, Time.deltaTime * _rotationSpeed);
            }


            float rotDiffY = Mathf.Abs(_horizontalRotate.transform.rotation.eulerAngles.y - rotHorzY);

            if (rotDiffY < 15f)
            {
                if (_isAttacking)
                {
                    Debug.DrawLine(drawPos, targetPos, Color.green, 0.1f);
                } else {

                    StartAttack(targetGO);
                    _isAttacking = true;
                }
            }
            else
            {
                Debug.DrawLine(drawPos, targetPos, Color.yellow, 0.1f);

                StopAttack();
                _isAttacking = false;
            }
        }

        public GameObject GetAttackTarget()
        {
            return _hitZone;
        }

    }

}
