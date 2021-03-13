using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQITP.Utility
{
    public class FaceCamera : MonoBehaviour
    {
        void Update()
        {
            transform.eulerAngles = Camera.main.transform.eulerAngles;
        }
    }
}
