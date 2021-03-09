using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameDevHQITP.Widgets
{
    public class MenuChoice : MonoBehaviour
    {
        public static event Action OnCancel;
        public static event Action OnAccept;

        public void OnClickAccept()
        {
            if (OnAccept != null)
            {
                OnAccept();
            }
        }

        public void OnClickCancel()
        {
            if (OnCancel != null)
            {
                OnCancel();
            }
        }
    }
}
