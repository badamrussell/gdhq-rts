using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Should I be updating it like this on every update?
 */
namespace GameDevHQITP.Widgets
{
    public class ProgressMeter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _amount;
        [SerializeField] private Transform _amountTransform;

        [SerializeField] private Color _healthyColor;
        [SerializeField] private Color _dangerColor;

        public float progressValue { get; set; }

        void Update()
        {
            float xScale = Mathf.Clamp(progressValue * 10f, 0.1f, 10f);
            if (xScale < 6f)
            {
                _amount.color = _dangerColor;
            } else
            {
                _amount.color = _healthyColor;
            }
            _amountTransform.localScale = new Vector3(xScale, _amountTransform.localScale.y, _amountTransform.localScale.z);
        }
    }
}
