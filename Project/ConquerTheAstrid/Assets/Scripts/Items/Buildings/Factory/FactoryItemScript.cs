using System.Collections;
using Core;
using Items.Buildings.Smelter;
using Items.Resource;
using NnUtils.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Buildings.Factory
{
    public class FactoryItemScript : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _itemToCraftAmount;
        [SerializeField] private NBar _craftNBar;

        [SerializeField] private Button _addButton;
        [SerializeField] private Button _subtractButton;
        
        private FactoryScript _factoryScript;
        
        [SerializeField] private TMP_Text _inputItemName;
        [SerializeField] private TMP_Text _outputItemName;
        
        [SerializeField] private Image _inputItemImage;
        [SerializeField] private Image _outputItemImage;

        private ResourcesScript.Types _inputItemType;
        private ResourcesScript.Types _outputItemType;
        
        private float _craftTime;
        
        private int _craftAmountForItem;
        private bool _crafting;

        private void Start()
        {
            _factoryScript = gameObject.GetComponentInParent<FactoryScript>();
        }

        public void AssignInfo(
            string inputItemName,
            string outputItemName,
            Sprite inputItemImage,
            Sprite outputItemImage,
            float craftTime,
            ResourcesScript.Types inputItemType,
            ResourcesScript.Types outputItemType
        )
        {
            _inputItemName.text = inputItemName;
            _outputItemName.text = outputItemName;
            _inputItemImage.sprite = inputItemImage;
            _outputItemImage.sprite = outputItemImage;
             _craftTime = craftTime;
            _inputItemType = inputItemType;
            _outputItemType = outputItemType;
        }

        public void Craft()
        {
            if (_itemToCraftAmount.text == "")
                return;
            if (_itemToCraftAmount.text == "0")
                return;
            
            if (int.TryParse(_itemToCraftAmount.text, out var smeltAmount))
                if (smeltAmount <= GameManager.Instance.ResourcesManagerScript.FindResouce(_inputItemType).Amount)
                {
                    _factoryScript.CraftButtonVisuals();
                    _crafting = true;
                    StartCoroutine(CraftRoutine());
                }
        }

        public void StopCrafting()
        {
            _crafting = false;
            StopCoroutine(CraftRoutine());
            _craftNBar.Value = 0.0f;
        }

        public void DeselectInputField()
        {
            if (int.TryParse(_itemToCraftAmount.text, out var smeltAmount))
            {
                if (smeltAmount <= 0)
                    _itemToCraftAmount.text = "0";
            }
        }

        public void AddButton()
        {
            if (_itemToCraftAmount.text != "")
            {
                _craftAmountForItem = int.Parse(_itemToCraftAmount.text);
                _craftAmountForItem++;
                _itemToCraftAmount.text = _craftAmountForItem.ToString();
            }
            
            else
            {
                _itemToCraftAmount.text = "1";
            }
        }

        public void SubtractButton()
        {
            if (_craftAmountForItem > 0)
            {
                if (_itemToCraftAmount.text != "")
                {
                    _craftAmountForItem = int.Parse(_itemToCraftAmount.text);
                    _craftAmountForItem--;
                    _itemToCraftAmount.text = _craftAmountForItem.ToString();
                }

                else
                {
                    _itemToCraftAmount.text = "0";
                }
            }
        }
        
        private IEnumerator CraftRoutine()
        {
            float time = 0.0f;
            float startSliderValue = _craftNBar.Value;

            while (time < _craftTime)
            {
                time += Time.deltaTime;
                float sliderValue = time / _craftTime;
                float currentSliderValue = Mathf.Lerp(startSliderValue, 100.0f, sliderValue);

                if (_crafting)
                {
                    _craftNBar.Value = currentSliderValue;
                }

                if (!_crafting)
                {
                    _craftNBar.Value = startSliderValue;
                    time = 0.0f;
                }

                yield return null;
            }

            _factoryScript.CraftResouces(_itemToCraftAmount.text, _inputItemType, _outputItemType);
            _factoryScript.StopCraftingButtonVisuals();
            _craftNBar.Value = 0.0f;
        }
    }
}
