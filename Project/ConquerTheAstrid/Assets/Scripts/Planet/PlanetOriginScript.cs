using System;
using System.Collections;
using UnityEngine;

namespace Planet
{
    public class PlanetOriginScript : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        private void Start()
        {
            StartCoroutine(RotateOriginRoutine());
        }

        private IEnumerator RotateOriginRoutine()
        {
            var rotAmount = Vector3.up * _rotationSpeed;
            while (true)
            {
                transform.Rotate(rotAmount * Time.deltaTime);
                yield return null;
            }
        }
    }
}