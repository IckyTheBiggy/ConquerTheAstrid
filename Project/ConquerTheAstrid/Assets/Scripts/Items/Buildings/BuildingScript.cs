using System;
using System.Collections.Generic;
using System.Collections;
using Core;
using Items.Resource;
using NnUtils.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Items.Buildings
{
    public class BuildingScript : MonoBehaviour, IInteractable
    {
        [Serializable]
        public struct RecipePrice
        {
            public List<ResourcesScript.PriceItem> PriceItems;
            public int Level;
        }
    
        [SerializeField] private BuildingSO _buildingSO;
        [SerializeField] private ParticleSystem[] smokeParticleSystems;
        [SerializeField] private AnimationCurve _selectAnimCurve;
        [SerializeField] private Transform _meshTransform;
        [SerializeField] private ParticleSystem _selectParticles;
        [SerializeField] private GameObject _menu;
        [SerializeField] private AudioManager.Sounds _sound;
        [SerializeField] private float _pitchVariation;

        private bool MachineRunning;
        
        private void IdlePolution()
        {
                GameManager.Instance.PollutionScript.AffectPollution(_buildingSO.IdlePollution);
        }

        private void SmokeParticlesEffects()
        {
            if (MachineRunning) 
                foreach (var t in smokeParticleSystems)
                {
                    var emission = t.emission;
                     emission.rateOverTime = 1.4f;
                }
            else
                foreach (var t in smokeParticleSystems)
                {
                    var emission = t.emission;
                    emission.rateOverTime = 0.8f;
                }
        }

        public void CloseUI()
        {
            _menu.SetActive(false);
        }

        public void Focus()
        {
            
        }

        public void Unfocus()
        {
            
        }

        public void Select()
        {
            _menu.SetActive(true);
            GameManager.Instance.AudioManager.PlaySFX(_sound, _pitchVariation);
            Animate(Vector3.zero, Vector3.zero, Vector3.one * Random.Range(0.05f, 0.1f), 0.5f, _selectAnimCurve);
            
            if (_selectParticles != null)
                _selectParticles.Play();
        }

        public void Deselect()
        {
            CloseUI();
        }

        /*
        private void MachineRunningShakeEffect()
        {
           _buildingMeshGameObject.transform.localPosition = 0.1f * new Vector3(
                Mathf.PerlinNoise(8 * Time.deltaTime, 1),
                Mathf.PerlinNoise(8 * Time.deltaTime, 2),
                Mathf.PerlinNoise(8 * Time.deltaTime, 3));
        }
        */
        
        private void Animate(Vector3 posDelta, Vector3 rotDelta, Vector3 scaleDelta, float time, AnimationCurve curve) 
        {
            Misc.RestartCoroutine(this, ref _animationRoutine, 
                AnimationRoutine(posDelta, Quaternion.Euler(rotDelta), Vector3.one + scaleDelta, time, curve));
        }
        private Coroutine _animationRoutine;
        
        private IEnumerator AnimationRoutine(Vector3 targetPos, Quaternion targetRot, Vector3 targetScale, float time, AnimationCurve curve)
        {
            float lerpPos = 0;
            var startRot = Quaternion.Euler(Vector3.zero);
            while (lerpPos < 1)
            {
                var t = curve.Evaluate(Misc.UpdateLerpPos(ref lerpPos, time));
                _meshTransform.localPosition = Vector3.LerpUnclamped(Vector3.zero, targetPos, t);
                _meshTransform.localRotation = Quaternion.LerpUnclamped(startRot, targetRot, t);
                _meshTransform.localScale = Vector3.LerpUnclamped(Vector3.one, targetScale, t);
                yield return null;
            }
            _animationRoutine = null;
        }
    }
    
    
}
