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
        public PlanetScript CurrentPlanet;
        private PlanetScript _previousPlanet;
        private List<PlanetScript> _planetScripts;
        [SerializeField] private float _transitionTime;
        [SerializeField] private Easings.Types _transitionEasing;

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
        
        public void SwitchPlanet(PlanetScript ps)
        {
            if (CurrentPlanet == ps) return;
            
            CurrentPlanet.SwitchFrom();
            _previousPlanet = CurrentPlanet;
            CurrentPlanet = ps;
            ps.SwitchTo();
            
            GameManager.Instance.AudioManager.PlaySFX(AudioManager.Sounds.PlanetSelect, 0.2f);
            //Misc.RestartCoroutine(this, ref _planetSwitchRoutine, PlanetSwitchRoutine(ps));
        }
    }
}
