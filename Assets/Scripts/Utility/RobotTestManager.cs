using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;

namespace GameDevHQITP.Utility
{

    public class RobotTestManager : MonoBehaviour
    {

        public Enemy enemy;

        public void Kill()
        {
            enemy.InitiateDeath();
        }

        public void ToggleActive()
        {
            enemy.gameObject.SetActive(!enemy.gameObject.activeSelf);
        }

        public void Reanimate()
        {
            enemy.InitiateActive();
        }
    }
}
