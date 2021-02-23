using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Managers;
using System;

namespace GameDevHQITP.Units
{
    public class PlayerHome : MonoBehaviour
    {
        public static event Action<GameObject> onReachedPlayerBase;

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
