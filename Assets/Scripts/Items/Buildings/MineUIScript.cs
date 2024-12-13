using Items.Buildings.Upgrades;
using TMPro;
using UnityEngine;

namespace Items.Buildings
{
    public class MineUIScript : MonoBehaviour
    {
        [SerializeField] private MineUpgrades _mineUpgradesScript;
        [SerializeField] private TMP_Text _currentLevelText;
        [SerializeField] private TMP_Text _upgradeCostText;

        public void UpgradeMineButton()
        {
            _mineUpgradesScript.UpgradeLevel();
        }

        private void UpdateUI()
        {
            _currentLevelText.text = _mineUpgradesScript._currentLevel.ToString();
            _upgradeCostText.text = _mineUpgradesScript._levels[_mineUpgradesScript._currentLevel].Cost.ToString();
        }

        private void Update()
        {
            UpdateUI();
        }
    }
}
