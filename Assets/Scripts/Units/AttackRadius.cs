using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameDevHQITP.Units
{
    public class AttackRadius : MonoBehaviour
    {
        public static event Action<GameObject, int> onTakeDamage;

        [SerializeField] private List<GameObject> _nearbyEnemies = new List<GameObject>();

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;

        [SerializeField] float _rotationSpeed = 2f;
        [SerializeField] GameObject _towerActionsGO;
        [SerializeField] ITowerActions _towerActions;
        [SerializeField] GameObject _targetedEnemy;

        [SerializeField] float _horizontalAngleOffset = 0f;
        [SerializeField] float _verticalAngleOffset = 0f;

        private void Start()
        {
            _towerActions = _towerActionsGO.GetComponent<ITowerActions>();
        }
        private void Update()
        {
            Vector3 targetPos;

            if (GetTargetPosition(out targetPos)) {
                RotateTurret(targetPos);
            }
        }

        private void OnEnable()
        {
            TargetArea.onTargetAreaDestroyed += RemoveTarget;
        }

        private void OnDisable()
        {
            TargetArea.onTargetAreaDestroyed -= RemoveTarget;
        }

        private void RemoveTarget(GameObject target)
        {
            if (_targetedEnemy == target)
            {
                _targetedEnemy = null;
            }

            _nearbyEnemies.RemoveAll(e => e == target);
        }

        private void RotateTurret(Vector3 targetPos)
        {
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
                if (_targetedEnemy == null)
                {
                    _towerActions.StartAttack(_nearbyEnemies[0]);
                    _targetedEnemy = _nearbyEnemies[0];
                    StartCoroutine("SendDamage");
                } else
                {
                    Debug.DrawLine(vertTrans, targetPos, Color.green, 0.1f);
                }
            } else
            {
                Debug.DrawLine(vertTrans, targetPos, Color.yellow, 0.1f);

                _towerActions.StopAttack();
                _targetedEnemy = null;
                StopCoroutine("SendDamage");
            }
        }

        private IEnumerator SendDamage()
        {
            while (_targetedEnemy != null)
            {
                yield return new WaitForSeconds(0.1f);
                onTakeDamage(_targetedEnemy, 2);
            }
        }

        private bool GetTargetPosition(out Vector3 targetPos)
        {
            if (_nearbyEnemies.Count > 0)
            {
                // Can probably be smarter about this
                // lock onto enemy
                // if enemy leaves attackRadius, select closest enemy & lock on
                targetPos = _nearbyEnemies[0].transform.position;
                return true;
            }

            targetPos = Vector3.zero;
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") { return; }

            _nearbyEnemies.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Enemy") { return; }

            int index = _nearbyEnemies.FindIndex(e => e == other.gameObject);
            if (index > -1)
            {
                if (_nearbyEnemies[index] == _targetedEnemy)
                {
                    _targetedEnemy = null;
                    StopCoroutine("SendDamage");
                }
                _nearbyEnemies.RemoveAt(index);
            }
        }
    }
}
