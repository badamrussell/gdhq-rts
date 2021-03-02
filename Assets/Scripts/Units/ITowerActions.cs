using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQITP.Units
{
    public interface ITowerActions
    {
        public void StartAttack(GameObject target);
        public void StopAttack();
    }
}
