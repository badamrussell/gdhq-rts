using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameDevHQITP.Units
{
    abstract public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private AttackRadius _attackRadius;
        [SerializeField] protected float _attackRadiusAmount = 15f;

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;
        [SerializeField] private GameObject _forwardDir;

        [SerializeField] float _rotationSpeed = 2f;

        [SerializeField] protected int _baseDamagePerSec = 20;

        [SerializeField] float _viewAngle = 30f;

        protected abstract void StopAttack();
        protected abstract void StartAttack(GameObject target);

        protected int _damageRate;
        protected bool _isAttacking;


        protected void Start()
        {
            _damageRate = Mathf.Max(_baseDamagePerSec / 10, 1);
            _isAttacking = false;
            _attackRadius.gameObject.transform.localScale = Vector3.one * _attackRadiusAmount;
        }

        protected void Update()
        {
            GameObject targetGO;

            // Vector3 leftPos = Quaternion.AngleAxis(-_viewAngle, Vector3.up) * _forwardDir.transform.forward * _attackRadiusAmount / 2f;
            // Debug.DrawRay(_horizontalRotate.transform.position, leftPos, Color.magenta, 0.01f);

            // Vector3 centerPos = _horizontalRotate.transform.forward * _attackRadiusAmount / 2f;
            // Debug.DrawRay(_horizontalRotate.transform.position, centerPos, Color.blue, 0.01f);

            // Vector3 rightPos = Quaternion.AngleAxis(_viewAngle, Vector3.up) * _forwardDir.transform.forward * _attackRadiusAmount / 2f;
            // Debug.DrawRay(_horizontalRotate.transform.position, rightPos, Color.magenta, 0.01f);

            // Vector3 frontPos = _forwardDir.transform.forward * _attackRadiusAmount / 2f;
            // Debug.DrawRay(_horizontalRotate.transform.position, frontPos, Color.green, 0.01f);

            if (_attackRadius.GetTarget(out targetGO))
            {
                LookAtTarget(targetGO);
            }
            else if (_isAttacking)
            {
                _isAttacking = false;
                StopAttack();

            } else
            {
                LookForward();
            }
        }

        private void LookForward()
        {

            Vector3 horzPos = _horizontalRotate.transform.forward;
            Vector3 targetPos = _forwardDir.transform.forward;
            Vector3 horzDiff = targetPos - horzPos;
            // Debug.Log($"LOOK: {horzDiff} is {horzDiff.magnitude}");

            if (horzDiff.magnitude < 0.01f)
            {
                return;
            }
            
            Quaternion diffHorzRot = Quaternion.LookRotation(horzDiff);
            _horizontalRotate.transform.rotation = Quaternion.Slerp(_horizontalRotate.transform.rotation, diffHorzRot, Time.deltaTime * _rotationSpeed);

        }

        private void LookAtTarget(GameObject targetGO)
        {
            Vector3 targetPos = targetGO.transform.position;

            // Rotate horizontally
            Vector3 horzPos = _horizontalRotate.transform.position;
            Vector3 horzDiff = targetPos - horzPos;
            horzDiff.y = 0f;
            Quaternion diffHorzRot = Quaternion.LookRotation(horzDiff);
            _horizontalRotate.transform.rotation = Quaternion.Slerp(_horizontalRotate.transform.rotation, diffHorzRot, Time.deltaTime * _rotationSpeed);


            if (_verticalRotate != null)
            {
                // Rotate vertically
                Vector3 vertTrans = _verticalRotate.transform.position;
                Vector3 vertDiff = targetPos - vertTrans;
                vertDiff.x = 0f;
                Quaternion diffVertRot = Quaternion.LookRotation(vertDiff);
                //Debug.DrawLine(vertTrans, targetPos, Color.magenta, 0.1f);
                _verticalRotate.transform.rotation = Quaternion.Slerp(_verticalRotate.transform.rotation, diffVertRot, Time.deltaTime * _rotationSpeed);
            }


            float angle = Vector3.Angle(_forwardDir.transform.forward, horzDiff);

            if (Mathf.Abs(angle) < _viewAngle)
            {
                if (_isAttacking)
                {
                    // Debug.DrawLine(_horizontalRotate.transform.position, targetPos, Color.yellow, 0.1f);
                }
                else
                {
                    StartAttack(targetGO);
                    _isAttacking = true;
                }
            }
            else
            {
                // Debug.DrawLine(_horizontalRotate.transform.position, targetPos, Color.red, 0.1f);
                if (_isAttacking)
                {
                    StopAttack();
                    _isAttacking = false;
                }
            }

        }
    }
}
