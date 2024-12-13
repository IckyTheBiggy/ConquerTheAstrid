using System.Collections.Generic;
using Core;
using Items.Resource;
using UnityEngine;

namespace Items.Buildings.Upgrades
{
    public class MineUpgrades : MonoBehaviour
    {
        [System.Serializable]
        public struct Levels
        {
            public int Cost;
            public float SpeedMultiplier;
            public float ResouceMultiplier;
            public float PollutionAmount;
        }

        [SerializeField] public List<Levels> _levels;
        [SerializeField] private MineScript _mineScript;

        [HideInInspector] public int _currentLevel;

        public bool UpgradeLevel()
        {
            if (GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Money).Amount <
                _levels[_currentLevel].Cost)
                return false;

            if (_currentLevel >= _levels.Count -1)
                return false;
            _currentLevel++;

            Debug.Log(_currentLevel);
            //_mineScript._mineTime /= _levels[_currentLevel].SpeedMultiplier;
            //Debug.Log(_mineScript._mineTime);
            GameManager.Instance.ResourcesManagerScript.AffectResource(ResourcesScript.Types.Money,
                -_levels[_currentLevel].Cost);
            return true;
        }
    }
}
