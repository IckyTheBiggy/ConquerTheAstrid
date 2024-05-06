using System.Collections;
using Core;
using Items.Resource;
using NnUtils.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Buildings.Smelter
{
    public class SmelterItemScript : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _itemToSmeltAmount;
        [SerializeField] private NBar _smelterNBar;

        [SerializeField] private Button _addButton;
        [SerializeField] private Button _subtractButton;
        
        private SmelterScript _smelterScript;
        
        [SerializeField] private TMP_Text _inputItemName;
        [SerializeField] private TMP_Text _outputItemName;
        
        [SerializeField] private Image _inputItemImage;
        [SerializeField] private Image _outputItemImage;

        private ResourcesScript.Types _inputItemType;
        private ResourcesScript.Types _outputItemType;
        
        private float _smeltTime;
        
        private int _smeltAmountForItem;
        private bool _smelting;

        private void Start()
        {
            _smelterScript = gameObject.GetComponentInParent<SmelterScript>();
        }

        public void AssignInfo(
            string inputItemName,
            string outputItemName,
            Sprite inputItemImage,
            Sprite outputItemImage,
            float smeltTime,
            ResourcesScript.Types inputItemType,
            ResourcesScript.Types outputItemType
        )
        {
            _inputItemName.text = inputItemName;
            _outputItemName.text = outputItemName;
            _inputItemImage.sprite = inputItemImage;
            _outputItemImage.sprite = outputItemImage;
            _smeltTime = smeltTime;
            _inputItemType = inputItemType;
            _outputItemType = outputItemType;
        }

        public void Smelt()
        {
            if (_itemToSmeltAmount.text == "")
                return;
            if (_itemToSmeltAmount.text == "0")
                return;
            
            if (int.TryParse(_itemToSmeltAmount.text, out var smeltAmount))
                if (smeltAmount <= GameManager.Instance.ResourcesManagerScript.FindResouce(_inputItemType).Amount)
                {
                    _smelterScript.SmeltButtonVisuals();
                    _smelting = true;
                    StartCoroutine(SmeltRoutine());
                }
        }

        public void StopSmelting()
        {
            _smelting = false;
            StopCoroutine(SmeltRoutine());
            _smelterNBar.Value = 0.0f;
        }

        public void DeselectInputField()
        {
            if (int.TryParse(_itemToSmeltAmount.text, out var smeltAmount))
            {
                if (smeltAmount <= 0)
                    _itemToSmeltAmount.text = "0";
            }
        }

        public void AddButton()
        {
            if (_itemToSmeltAmount.text != "")
            {
                _smeltAmountForItem = int.Parse(_itemToSmeltAmount.text);
                _smeltAmountForItem++;
                _itemToSmeltAmount.text = _smeltAmountForItem.ToString();
            }
            
            else
            {
                _itemToSmeltAmount.text = "1";
            }
        }

        public void SubtractButton()
        {
            if (_smeltAmountForItem > 0)
            {
                if (_itemToSmeltAmount.text != "")
                {
                    _smeltAmountForItem = int.Parse(_itemToSmeltAmount.text);
                    _smeltAmountForItem--;
                    _itemToSmeltAmount.text = _smeltAmountForItem.ToString();
                }

                else
                {
                    _itemToSmeltAmount.text = "0";
                }
            }
        }
        
        private IEnumerator SmeltRoutine()
        {
            float time = 0.0f;
            float startSliderValue = _smelterNBar.Value;

            while (time < _smeltTime)
            {
                time += Time.deltaTime;
                float sliderValue = time / _smeltTime;
                float currentSliderValue = Mathf.Lerp(startSliderValue, 100.0f, sliderValue);

                if (_smelting)
                {
                    _smelterNBar.Value = currentSliderValue;
                }

                if (!_smelting)
                {
                    _smelterNBar.Value = startSliderValue;
                    time = 0.0f;
                }

                yield return null;
            }

            _smelterScript.SmeltResouces(_itemToSmeltAmount.text, _inputItemType, _outputItemType);
            _smelterScript.StopSmeltingButtonVisuals();
            _smelterNBar.Value = 0.0f;
        }
    }
}
