using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Camera
{
    public class CameraScript : MonoBehaviour
    {
        private bool _isMoving;
        private Vector3 _prevMousePos;
        [HideInInspector] public float CurrentZoom;
        [SerializeField] private float _sensitivity, _zoomSensitivity;
        public float ZoomSensitivityMultiplier = 1;
        public float ZoomMin, ZoomMax;
        public Vector3 RotationCenter;
        private float _previousZoom, _targetZoom;

        private void Awake() => _previousZoom = _targetZoom = CurrentZoom;

        private void Start()
        { 
            UpdateValuesFromCurrentPlanet();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse2) && !Misc.IsPointerOverUI)
            {
                _prevMousePos = Input.mousePosition;
                _isMoving = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse2)) _isMoving = false;
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
            _previousZoom = _targetZoom = CurrentZoom = Misc.Remap(CurrentZoom, zMin, zMax, ps.ZoomMin, ps.ZoomMax);
        }

        public void OnPlanetChanged()
        {
            _prevMousePos = Input.mousePosition;
        }
        
        private void Move()
        {
            var rotation = DesktopRotation();
            _prevMousePos = rotation == Vector3.zero ? _prevMousePos : Input.mousePosition;
            transform.position = RotationCenter;
            if (_isMoving)
            {
                transform.Rotate(Vector3.right, rotation.y);
                transform.Rotate(Vector3.up, -rotation.x, Space.World);
            }

            transform.Translate(0, 0, CurrentZoom);
        }

        private Vector3 DesktopRotation() =>
            Input.GetKey(KeyCode.Mouse2)
                ? (_prevMousePos - Input.mousePosition) * _sensitivity
                : Vector3.zero;

        private void Zoom()
        {
            StandaloneZoom();
        }

        private void StandaloneZoom()
        {
            var delta = Input.GetAxisRaw("Mouse ScrollWheel");
            if (delta == 0) return;
            _previousZoom = CurrentZoom;
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
                CurrentZoom = Mathf.Lerp(_previousZoom, _targetZoom, t);
                yield return null;
            }

            _zoomRoutine = null;
        }
    }
}
