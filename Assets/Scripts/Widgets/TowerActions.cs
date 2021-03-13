using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.Widgets
{
    public class TowerActions : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        //[SerializeField] private GameObject 
        //private void Start()
        //{
        //    OnHide();
        //}

        //private void OnEnable()
        //{
        //    OnShow();
        //}

        public void OnShow()
        {
            Debug.Log("SHOW TOWER ACTIONS");
            _animator.SetBool("IsVisible", true);
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
        }

        public void OnHide()
        {
            _animator.SetBool("IsVisible", false);
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
        }

        public void OnSell()
        {
            Debug.Log("SHOW CONFIRM SELL");

            _animator.SetBool("IsDismantle", true);
            _animator.SetBool("IsUpgrade", false);
        }

        public void OnUpgrade()
        {
            Debug.Log("SHOW CONFIRM UPGRADE");
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", true);
        }

        public void OnConfirmSell()
        {
            Debug.Log("CONFIRM DISMANTLE");
        }

        public void OnConfirmUpgrade()
        {
            Debug.Log("CONFIRM UPGRADE");
        }

        public void OnCancel()
        {
            _animator.SetBool("IsDismantle", false);
            _animator.SetBool("IsUpgrade", false);
        }
    }
}
