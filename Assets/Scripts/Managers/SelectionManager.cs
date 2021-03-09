using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevHQITP.Utility;

namespace GameDevHQITP.Managers
{
    public class SelectionManager : MonoSingleton<SelectionManager>
    {
        public static event Action<GameObject> OnSelectedTower;
        [SerializeField] List<GameObject> _selectedTowers = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnSelectTower(GameObject go)
        {
            _selectedTowers.Clear();
            _selectedTowers.Add(go);
            OnSelectedTower(go);
        }
    }
}
