using System.Collections;
using Assets.NnUtils.Scripts;
using NnUtils.Scripts;
using UnityEngine;

namespace Camera
{
    public class NewCameraScript : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private Easings.Types _rotationEasing = Easings.Types.ExpoOut;
        [SerializeField] private float _zoomSpeed = 0.5f;
        [SerializeField] private Easings.Types _zoomEasing = Easings.Types.ExpoOut;
        
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
            var startRot = transform.localRotation.eulerAngles;
            var delta = Vector2.zero;

            while (true)
            {
                var newDelta = (Misc.GetPointerPos() - startMousePos) * _rotationSpeed;
                if (delta == newDelta) { yield return null; continue; }
                delta = newDelta;
                
                Vector3 targetRot = new(startRot.x - newDelta.y, startRot.y + newDelta.x, startRot.z);
                Misc.RestartCoroutine(this, ref _lerpRotationRoutine, LerpRotationRoutine(targetRot));
                yield return null;
            }
        }

        private Coroutine _lerpRotationRoutine;
        private IEnumerator LerpRotationRoutine(Vector3 targetRotEuler)
        {
            var startRotEuler = transform.localRotation.eulerAngles;
            startRotEuler.z = targetRotEuler.z = 0;
            var startRot = Quaternion.Euler(startRotEuler);
            var targetRot = Quaternion.Euler(targetRotEuler);
            float lerpPos = 0;

            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 1, false, _rotationEasing);
                transform.localRotation = Quaternion.Lerp(startRot, targetRot, t);
                var euler = transform.localRotation.eulerAngles;
                euler.z = 0;
                transform.localRotation = Quaternion.Euler(euler);
                yield return null;
            }

            _lerpRotationRoutine = null;
        }
    }
}