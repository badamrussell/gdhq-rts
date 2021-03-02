using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameDevHQITP.Units
{
    // Detects "attackable" objects within zone
    // Chooses target
    // Aims at target
    public class AttackRadius : MonoBehaviour
    {
        public static event Action<GameObject, int> onTakeDamage;

        [SerializeField] private List<GameObject> _nearbyTargets = new List<GameObject>();

        [SerializeField] private GameObject _horizontalRotate;
        [SerializeField] private GameObject _verticalRotate;

        [SerializeField] float _rotationSpeed = 2f;
        [SerializeField] GameObject _towerActionsGO;
        [SerializeField] GameObject _targetedArea;

        // TO_DO: this usage seems weird. better way to notify tower to start/stop?
        // probably an event,
        [SerializeField] ITowerActions _towerActions;

        [SerializeField] float _horizontalAngleOffset = 0f;
        [SerializeField] float _verticalAngleOffset = 0f;
        [SerializeField] int _baseDamagePerSec = 20;

        private int _damageRate;

        private void Start()
        {
            _damageRate = Mathf.Max(_baseDamagePerSec / 10, 1);
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
            if (_targetedArea == target)
            {
                _targetedArea = null;
            }

            _nearbyTargets.RemoveAll(e => e == target);
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
                if (_targetedArea == null)
                {
                    //TO_DO: raycast check instead of assumption
                    _towerActions.StartAttack(_nearbyTargets[0]);
                    _targetedArea = _nearbyTargets[0];
                    Debug.DrawLine(vertTrans, targetPos, Color.yellow, 0.1f);
                    StartCoroutine("SendDamage");
                } else
                {
                    Debug.DrawLine(vertTrans, targetPos, Color.green, 0.1f);
                }
            } else
            {
                Debug.DrawLine(vertTrans, targetPos, Color.yellow, 0.1f);

                _towerActions.StopAttack();
                _targetedArea = null;
                StopCoroutine("SendDamage");
            }
        }

        private IEnumerator SendDamage()
        {
            while (_targetedArea != null)
            {
                yield return new WaitForSeconds(0.1f);
                onTakeDamage(_targetedArea, _damageRate);
            }
        }

        private bool GetTargetPosition(out Vector3 targetPos)
        {
            if (_nearbyTargets.Count > 0)
            {
                // TO_DO: Instead of taking first, is there a better algorithm?
                // if enemy leaves attackRadius, select closest enemy & lock on
                targetPos = _nearbyTargets[0].transform.position;
                return true;
            }

            targetPos = Vector3.zero;
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Attackable") { return; }

            _nearbyTargets.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Attackable") { return; }

            int index = _nearbyTargets.FindIndex(e => e == other.gameObject);
            if (index > -1)
            {
                if (_nearbyTargets[index] == _targetedArea)
                {
                    _targetedArea = null;
                    StopCoroutine("SendDamage");
                }
                _nearbyTargets.RemoveAt(index);
            }
        }
    }
}
