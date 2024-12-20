using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraScript : MonoBehaviour
    {
        private float _targetZoom;

        private Vector2 _zoomMinMax;
        [SerializeField] private float _zoomSpeed = 0.5f;
        [SerializeField] private float _zoomLerpTime = 1;
        [SerializeField] private Easings.Types _zoomEasing = Easings.Types.ExpoOut;

        private void Awake()
        {
            var planetManager = GameManager.Instance.PlanetManagerScript;
            _targetZoom = transform.localPosition.z; //Update with the current planet instead
            planetManager.OnCurrentPlanetChanged += () => SetMinMax(planetManager.CurrentPlanet.ZoomMin, planetManager.CurrentPlanet.ZoomMax);
        }

        private void Update() => Zoom(InputSystem.actions["Zoom"].ReadValue<float>());

        private void Zoom(float amount, bool forceUpdate = false)
        {
            if (!forceUpdate && amount == 0) return;
            _targetZoom = Mathf.Clamp(_targetZoom + (amount * _zoomSpeed), _zoomMinMax.x, _zoomMinMax.y);
            Misc.RestartCoroutine(this, ref _lerpZoomRoutine, LerpZoomRoutine());
        }
        
        private Coroutine _lerpZoomRoutine;
        private IEnumerator LerpZoomRoutine()
        {
            var startZoom = transform.localPosition.z;
            float lerpPos = 0;

            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, _zoomLerpTime, false, _zoomEasing);
                var zoom = Mathf.Lerp(startZoom, _targetZoom, t);
                transform.localPosition = new (transform.localPosition.x, transform.localPosition.y, zoom);
                yield return null;
            }
            
            _lerpZoomRoutine = null;
        }

        private void SetMinMax(float min, float max)
        {
            _zoomMinMax = new(min, max);
            Zoom(0, true);
        }
    }
}
