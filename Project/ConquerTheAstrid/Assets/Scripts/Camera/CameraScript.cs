using System;
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
        private float _currentZoom, _targetZoom;

        [SerializeField] private Vector2 _zoomMinMax;
        [SerializeField] private float _zoomSpeed = 0.5f;
        [SerializeField] private Easings.Types _zoomEasing = Easings.Types.ExpoOut;

        private void Update() => Zoom(1);

        private void Zoom(float amount)
        {
            Debug.Log(amount);
        }
        
        private Coroutine _lerpZoomRoutine;
        private IEnumerator LerpZoomRoutine()
        {
            yield return null;
        }

        private void SetMinMax()
        {
            
        }
    }
}
