using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameDevHQITP.ScriptableObjects;

namespace GameDevHQITP.Widgets
{
    public class WarBucks : MonoBehaviour
    {
        [SerializeField] private IntVariable _warBucks;
        [SerializeField] private Text _text;

        void Start()
        {
            _text = GetComponent<Text>();
            if (!_text)
            {
                Debug.LogError("WarBucks missing text behavior.");
            }
        }

        void Update()
        {
            _text.text = _warBucks.value.ToString();
        }
    }
}
