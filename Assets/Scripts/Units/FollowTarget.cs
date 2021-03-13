using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameDevHQITP.Units
{
    abstract public class FollowTarget : MonoBehaviour
    {
        public static Action<GameObject, int> OnTakeDamage;

        [SerializeField] private AttackRadius _attackRadius;
        [SerializeField] protected float _attackRadiusAmount = 15f;

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;

        [SerializeField] float _rotationSpeed = 2f;

        [SerializeField] float _horizontalAngleOffset = 0f;
        [SerializeField] float _verticalAngleOffset = 0f;
        [SerializeField] int _baseDamagePerSec = 20;

        protected abstract void StopAttack();
        protected abstract void StartAttack(GameObject target);

        protected int _damageRate;
        protected bool _isAttacking;

        [SerializeField] private Vector3 _defaultRotation;

        protected void Start()
        {
            _damageRate = Mathf.Max(_baseDamagePerSec / 10, 1);
            _isAttacking = false;
            _attackRadius.gameObject.transform.localScale = Vector3.one * _attackRadiusAmount;
            _defaultRotation = _horizontalRotate.transform.rotation.eulerAngles;
        }

        protected void Update()
        {
            GameObject targetGO;

            if (_attackRadius.GetTarget(out targetGO))
            {
                LookAtTarget(targetGO);
            }
            else if (_isAttacking)
            {
                _isAttacking = false;
                StopAttack();
            }
        }

        private void LookAtTarget(GameObject targetGO)
        {
            Vector3 targetPos = targetGO.transform.position;
            float offsetY = -90f;
            float offsetZ = 90f;
            float offsetX = 0f;
            Vector3 offsetRot = new Vector3(offsetX, offsetY, offsetZ);

            // Rotate horizontally
            Vector3 horzPos = _horizontalRotate.transform.position;
            Vector3 horzDiff = horzPos - targetPos;
            Quaternion diffHorzRot = Quaternion.LookRotation(horzDiff);
            float rotHorzY = diffHorzRot.eulerAngles.y + _horizontalAngleOffset;
            float rotHorzZ = diffHorzRot.eulerAngles.z + _horizontalAngleOffset;
            Quaternion rotHorz = Quaternion.Euler(0, 0, rotHorzZ);
            Vector3 targetAngles = _horizontalRotate.transform.rotation.eulerAngles;
            Quaternion targetHorz = _horizontalRotate.transform.rotation;
            targetHorz.eulerAngles = new Vector3(targetAngles.x, targetAngles.y, rotHorzZ);

            Vector3 currentRotation = diffHorzRot.eulerAngles + offsetRot;
            currentRotation.z = diffHorzRot.eulerAngles.y;
            //Debug.Log($"rot: {_defaultRotation} > {horzDiff}");
            Debug.DrawLine(horzPos, targetPos, Color.cyan, 0.1f);
            //_horizontalRotate.transform.rotation = Quaternion.Slerp(_horizontalRotate.transform.rotation, targetHorz, Time.deltaTime * _rotationSpeed);
            //_horizontalRotate.transform.rotation = Quaternion.Euler(currentRotation);// Quaternion.Euler(targetHorz.x, targetHorz.y, targetHorz.z);
            //Quaternion.Slerp(_horizontalRotate.transform.rotation, targetHorz, Time.deltaTime * _rotationSpeed);
            //Vector3 nextRotation = new Vector3(0, 0, 30f) + offsetRot;
            //Vector3 nextRotation = _horizontalRotate.transform.rotation.eulerAngles;
            Vector3 nextRotation = _defaultRotation + horzDiff;
            Debug.Log($"rot: {_defaultRotation} > {nextRotation}");


            _horizontalRotate.transform.rotation = diffHorzRot;// Quaternion.Euler(nextRotation);// Quaternion.Euler(targetHorz.x, targetHorz.y, targetHorz.z);

            //Vector3 drawPos = horzTrans;

            //if (_verticalRotate != null)
            //{
            //    // Rotate vertically
            //    Vector3 vertTrans = _verticalRotate.transform.position;
            //    drawPos = vertTrans;
            //    Vector3 vertDiff = targetPos - vertTrans;
            //    Quaternion diffRot = Quaternion.LookRotation(vertDiff);
            //    float rotX = diffRot.eulerAngles.x + _verticalAngleOffset;
            //    Quaternion rotVert = Quaternion.Euler(rotX, rotHorzY, 0);
            //    Debug.DrawLine(vertTrans, targetPos, Color.magenta, 0.1f);
            //    _verticalRotate.transform.rotation = Quaternion.Slerp(_verticalRotate.transform.rotation, rotVert, Time.deltaTime * _rotationSpeed);
            //}


            //float rotDiffY = Mathf.Abs(_horizontalRotate.transform.rotation.eulerAngles.y - rotHorzY);

            //if (rotDiffY < 15f)
            //{
            //    if (_isAttacking)
            //    {
            //        //Debug.DrawLine(drawPos, targetPos, Color.green, 0.1f);
            //    }
            //    else
            //    {

            //        StartAttack(targetGO);
            //        _isAttacking = true;
            //    }
            //}
            //else
            //{
            //    //Debug.DrawLine(drawPos, targetPos, Color.red, 0.1f);

            //    StopAttack();
            //    _isAttacking = false;
            //}
        }
    }
}
