using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;
using System;

namespace Camera
{
    public class CameraScript : MonoBehaviour
    {
        private bool _isMoving;
        private Vector3 _prevMousePos;
        [HideInInspector] public float _currentZoom;
        [SerializeField] private float _sensitivity, _zoomSensitivity;
        public float ZoomSensitivityMultiplier = 1;
        public float ZoomMin, ZoomMax;
        public Vector3 RotationCenter;
        private float _previousZoom, _targetZoom;

        private void Awake() => _previousZoom = _targetZoom = _currentZoom;

        private void Start()
        { 
            UpdateValuesFromCurrentPlanet();
        }
        
        private void Update()
        {
#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Mouse2) && !Misc.IsPointerOverUI)
            {
                _prevMousePos = Input.mousePosition;
                _isMoving = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse2)) _isMoving = false;
#endif
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                _isMoving = false;
            if (Input.touchCount > 2) return;
            if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began) && !Misc.IsPointerOverUI)
            {
                _prevMousePos = Input.GetTouch(0).position;
                _isMoving = true;
            }
            if (Input.touchCount == 2 && !Misc.IsPointerOverUI)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    _prevMousePos = Input.GetTouch(1).position;
                if (Input.GetTouch(1).phase == TouchPhase.Ended)
                    _prevMousePos = Input.GetTouch(0).position;
            }
#endif
            if (!Misc.IsPointerOverUI) Zoom();
            Move();
        }

        public void UpdateValuesFromCurrentPlanet()
        {
            var ps = GameManager.Instance.PlanetManagerScript.CurrentPlanet;
            RotationCenter = ps.transform.position;
            var zMin = ZoomMin;
            var zMax = ZoomMax;
            ZoomMin = ps.ZoomMin;
            ZoomMax = ps.ZoomMax;
            ZoomSensitivityMultiplier = ps.ZoomSensitivityMultiplier;
            _previousZoom = _targetZoom = _currentZoom = Misc.Remap(_currentZoom, zMin, zMax, ps.ZoomMin, ps.ZoomMax);
        }

        public void OnPlanetChanged()
        {
            #if UNITY_STANDALONE
            _prevMousePos = Input.mousePosition;
            #endif
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
                _prevMousePos = Input.GetTouch(0).position;
#endif
        }
        
        private void Move()
        {
            Vector3 rotation = new();
#if UNITY_STANDALONE
            rotation = DesktopRotation();
            _prevMousePos = rotation == Vector3.zero ? _prevMousePos : Input.mousePosition;
#endif
#if UNITY_ANDROID || UNITY_IOS
            rotation = MobileRotation();
            _prevMousePos = rotation == Vector3.zero ? _prevMousePos : Input.GetTouch(0).position;
#endif
            transform.position = RotationCenter;
            if (_isMoving)
            {
                transform.Rotate(Vector3.right, rotation.y);
                transform.Rotate(Vector3.up, -rotation.x, Space.World);
            }

            transform.Translate(0, 0, _currentZoom);
        }

        private Vector3 DesktopRotation() =>
            Input.GetKey(KeyCode.Mouse2)
                ? (_prevMousePos - Input.mousePosition) * _sensitivity
                : Vector3.zero;

        private Vector3 MobileRotation()
        {
            if (Input.touchCount < 1 || Input.touchCount > 1 || Input.GetTouch(0).phase == TouchPhase.Began)
                return Vector3.zero;
            return (_prevMousePos - (Vector3)Input.GetTouch(0).position) * _sensitivity;
        }

        private void Zoom()
        {
#if UNITY_STANDALONE
            StandaloneZoom();
#endif
#if UNITY_ANDROID || UNITY_IOS
            MobileZoom();
#endif
        }

        private void StandaloneZoom()
        {
            var delta = Input.GetAxisRaw("Mouse ScrollWheel");
            if (delta == 0) return;
            _previousZoom = _currentZoom;
            _targetZoom += delta * _zoomSensitivity * ZoomSensitivityMultiplier;
            _targetZoom = Mathf.Clamp(_targetZoom, ZoomMin, ZoomMax);
            if (_zoomRoutine != null)
            {
                StopCoroutine(_zoomRoutine);
                _zoomRoutine = null;
            }
            _zoomRoutine = StartCoroutine(ZoomRoutine());
        }

        private Coroutine _zoomRoutine;

        private IEnumerator ZoomRoutine()
        {
            float lerpPos = 0;
            
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime;
                lerpPos = Mathf.Clamp01(lerpPos);
                var t = Easings.Ease(lerpPos, Easings.Types.ExpoOut);
                _currentZoom = Mathf.Lerp(_previousZoom, _targetZoom, t);
                yield return null;
            }

            _zoomRoutine = null;
        }

        private void MobileZoom()
        {
            if (Input.touchCount != 2) return;
            var t0 = Input.GetTouch(0);
            var t1 = Input.GetTouch(1);
            var t0prevPos = t0.position - t0.deltaPosition;
            var t1prevPos = t1.position - t1.deltaPosition;
            var prevMagnitude = (t0prevPos - t1prevPos).magnitude;
            var currentMagnitude = (t0.position - t1.position).magnitude;
            var diff = currentMagnitude - prevMagnitude;
            _currentZoom += diff * .01f * _zoomSensitivity * ZoomSensitivityMultiplier;
            _currentZoom = Mathf.Clamp(_currentZoom, ZoomMin, ZoomMax);
        }
    }
}
