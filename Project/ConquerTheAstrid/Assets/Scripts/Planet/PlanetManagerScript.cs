using System;
using System.Collections;
using System.Collections.Generic;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Planet
{
    public class PlanetManagerScript : MonoBehaviour
    {
        private PlanetScript _currentPlanet;
        public PlanetScript CurrentPlanet
        {
            get => _currentPlanet;
            private set
            {
                if (_currentPlanet == value) return;
                _currentPlanet = value;
                OnCurrentPlanetChanged?.Invoke();
            }
        }
        public Action OnCurrentPlanetChanged;
        
        private List<PlanetScript> _planetScripts;
        [SerializeField] private float _transitionTime;
        public float TransitionTime => _transitionTime;
        [SerializeField] private Easings.Types _transitionEasing;
        public Easings.Types TransitionEasing => _transitionEasing;

        private void Awake()
        {
            _planetScripts = new();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).TryGetComponent<PlanetScript>(out var ps)) continue;
                _planetScripts.Add(ps);
            }
            CurrentPlanet = _planetScripts[0];
        }

        private void Start() => CurrentPlanet = _planetScripts[0]; //Done in start so other scripts can listen to event in Awake

        public void SwitchPlanet(PlanetScript ps)
        {
            if (CurrentPlanet == ps) return;
            
            CurrentPlanet.SwitchFrom();
            CurrentPlanet = ps;
            ps.SwitchTo();
            
            GameManager.Instance.AudioManager.PlaySFX(AudioManager.Sounds.PlanetSelect, 0.2f);
        }
    }
}
