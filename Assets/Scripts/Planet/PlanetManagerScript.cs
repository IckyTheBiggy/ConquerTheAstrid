using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            _planetScripts = GetComponentsInChildren<PlanetScript>().ToList();
        }

        private void Start()
        {
            CurrentPlanet = _planetScripts[0];
            SwitchPlanet(CurrentPlanet);
        }

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
