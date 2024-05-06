using NnUtils.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace Items.Plants
{
    public class PlantInfoScript : InfoScript
    {
        [SerializeField] [ColorUsage(true)] private Color32 _unfertilizedColor, _fertilizedColor;
        [SerializeField] private NBar _healthNBar, _maturityNBar;
        [SerializeField] private Button _fertilizeButton;
        [SerializeField] private Image _fertilizeButtonBackground;
        [SerializeField] private TMP_Text _fertilizeButtonText;

        private PlantScript _plantScript;
        
        public void Fertilize()
        {
            if (_plantScript.IsFertilized) return;
            _plantScript.IsFertilized = true;
            FertilizeUI();
        }

        private void FertilizeUI()
        {
            _fertilizeButtonText.text = "Fertilized";
            _fertilizeButtonBackground.color = _fertilizedColor;
            _fertilizeButton.interactable = false;
        }

        private void UnfertilizeUI()
        {
            _fertilizeButtonText.text = "Fertilize";
            _fertilizeButtonBackground.color = _unfertilizedColor;
            _fertilizeButton.interactable = true;
        }

        public void Show(PlantScript ps)
        {
            gameObject.SetActive(true);
            _plantScript = ps;
            _healthNBar.Max = _plantScript.PlantSO.Health;
            _healthNBar.Value = _plantScript.CurrentHealth;
            if (_plantScript.IsFertilized) FertilizeUI(); else UnfertilizeUI();
            UpdateMaturity();
            _plantScript.OnMaturityChanged += UpdateMaturity;
        }

        private void UpdateMaturity()
        {
            _maturityNBar.Value = _plantScript.Maturity * 100;
            _fertilizeButton.interactable = _plantScript.Maturity < 1;
        }
        
        public void Harvest()
        {
            _plantScript.Harvest();
        }
    }
}
