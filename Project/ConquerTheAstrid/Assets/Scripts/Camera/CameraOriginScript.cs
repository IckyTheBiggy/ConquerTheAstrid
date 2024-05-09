using System;
using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Camera
{
    public class CameraOriginScript : MonoBehaviour
    {
        private Vector2 _currentRot, _targetRot;
        
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private Easings.Types _rotationEasing = Easings.Types.ExpoOut;

        private void Start()
        {
            GameManager.Instance.PlanetManagerScript.OnCurrentPlanetChanged += MoveToPlanet;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Misc.StopCoroutine(this, ref _lerpRotationRoutine);
                Misc.RestartCoroutine(this, ref _rotateRoutine, RotateRoutine());
            }

            if (Input.GetKeyUp(KeyCode.Mouse2)) Misc.StopCoroutine(this, ref _rotateRoutine);
        }

        private Coroutine _rotateRoutine;
        private IEnumerator RotateRoutine()
        {
            var startMousePos = Misc.GetPointerPos();
            var startRot = _currentRot;
            var delta = Vector2.zero;

            while (true)
            {
                var newDelta = (Misc.GetPointerPos() - startMousePos) * _rotationSpeed;
                if (delta == newDelta)
                {
                    yield return null;
                    continue;
                }
                delta = newDelta;

                Vector2 targetRot = new(startRot.x - newDelta.y, startRot.y + newDelta.x);
                Misc.RestartCoroutine(this, ref _lerpRotationRoutine, LerpRotationRoutine(targetRot));
                yield return null;
            }
        }

        private Coroutine _lerpRotationRoutine;
        private IEnumerator LerpRotationRoutine(Vector2 targetRotEuler)
        {
            var startRotEuler = _currentRot;
            float lerpPos = 0;

            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 1, false, _rotationEasing);
                _currentRot = Vector2.Lerp(startRotEuler, targetRotEuler, t);
                transform.localRotation = Quaternion.Euler(_currentRot);
                yield return null;
            }

            _lerpRotationRoutine = null;
        }

        private void MoveToPlanet() => Misc.RestartCoroutine(this, ref _moveToPlanetRoutine, MoveToPlanetRoutine());
        private Coroutine _moveToPlanetRoutine;
        private IEnumerator MoveToPlanetRoutine()
        {
            var startPos = transform.localPosition;
            var planetManager = GameManager.Instance.PlanetManagerScript;
            var targetPos = planetManager.CurrentPlanet.transform.localPosition;
            var time = planetManager.TransitionTime;
            var easing = planetManager.TransitionEasing;
            float lerpPos = 0;
            
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, time, false, easing);
                transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }
            _moveToPlanetRoutine = null;
        }
    }
}