using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Utility;
using GameDevHQITP.Units;

namespace GameDevHQITP.Managers
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private float _zoomSpeed = 5f;
        [SerializeField] private float _maxZoom = 50f;
        [SerializeField] private float _minZoom = 15f;

        [SerializeField] private float _keyboardPanSpeed = 0.1f;
        [SerializeField] private float _minX = -45f;
        [SerializeField] private float _maxX = -5f;
        [SerializeField] private float _minZ = -22f;
        [SerializeField] private float _maxZ = 10f;

        [SerializeField] private float _mouseEdgePanSpeed = 0.1f;
        [SerializeField] private float _screenEdgePanningOffset = 100f;
        [SerializeField] private float _screenEdgePanningBottomOffset = 50f;
        [SerializeField] private float _screenEdgeGutter = 1f;

        private float[] _screenLimitsHorz = new float[4];
        private float[] _screenLimitsVert = new float[4];

        private void Start()
        {
            _screenLimitsHorz[0] = _screenEdgeGutter;
            _screenLimitsHorz[1] = 0 + _screenEdgePanningOffset;
            _screenLimitsHorz[2] = Screen.width - _screenEdgePanningOffset;
            _screenLimitsHorz[3] = Screen.width - _screenEdgeGutter;

            _screenLimitsVert[0] = 0 + _screenEdgeGutter;
            _screenLimitsVert[1] = 0 + _screenEdgePanningBottomOffset;
            _screenLimitsVert[2] = Screen.height - _screenEdgePanningOffset;
            _screenLimitsVert[3] = Screen.height - _screenEdgeGutter;
        }

        void Update()
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            if (Input.GetKey(KeyCode.W))
            {
                pos.z += _keyboardPanSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                pos.z -= _keyboardPanSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pos.x -= _keyboardPanSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += _keyboardPanSpeed;
            }


            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            if (mouseX > _screenLimitsHorz[0] && mouseX < _screenLimitsHorz[1])
            {
                pos.x -= _mouseEdgePanSpeed;
            }
            else if (mouseX > _screenLimitsHorz[2] && mouseX < _screenLimitsHorz[3])
            {
                pos.x += _mouseEdgePanSpeed;
            }

            if (mouseY > _screenLimitsVert[0] && mouseY < _screenLimitsVert[1])
            {
                pos.z -= _mouseEdgePanSpeed;
            }
            else if (mouseY > _screenLimitsVert[2] && mouseY < _screenLimitsVert[3])
            {
                pos.z += _mouseEdgePanSpeed;
            }

            float scrollValue = Input.GetAxis("Mouse ScrollWheel");

            if (scrollValue != 0f)
            {
                pos.y += scrollValue * _zoomSpeed;
                pos.y = Mathf.Clamp(pos.y, _minZoom, _maxZoom);
            }


            pos.x = Mathf.Clamp(pos.x, _minX, _maxX);
            pos.z = Mathf.Clamp(pos.z, _minZ, _maxZ);
            transform.position = pos;
        }

    }
}