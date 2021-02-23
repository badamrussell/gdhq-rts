using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Managers;
using System;

namespace GameDevHQITP.Units
{
    public class PlayerHome : MonoBehaviour
    {
        public delegate void OnReachedPlayerBase(GameObject go);
        public static event OnReachedPlayerBase onReachedPlayerBase;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                if (onReachedPlayerBase != null)
                {
                    onReachedPlayerBase(other.gameObject);
                }
            }
        }
    }
}
