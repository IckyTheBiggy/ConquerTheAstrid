using System;
using System.Collections.Generic;
using Core;
using Items.Resource;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Items.Buildings.Smelter
{
    public class SmelterScript : MonoBehaviour
    {
        [Serializable]
        public struct SmelterItemStruct
        {
            public string InputItemName;
            public string OutputItemName;
            public Sprite InputItemImage;
            public Sprite OutputItemImage;
            public float SmeltTime;
            public ResourcesScript.Types InputItemType;
            public ResourcesScript.Types OutputItemType;
        }
        
        [SerializeField] private List<SmelterItemStruct> _items;
        [SerializeField] private SmelterItemScript _smelterItemPrefab;
        [SerializeField] private Transform _itemHolder;
        
        [SerializeField] private ResourceSO _inputItemSO;
        [SerializeField] private ResourceSO _outPutItemSO;
    
        [SerializeField] private float _smeltTime;
    
        [SerializeField] private Color32 _smeltColor;
        [SerializeField] private Color32 _smeltingColor;
    
        [SerializeField] private TMP_Text _smeltButtonText;
        [SerializeField] private Image _smeltButtonBackground;

        private List<SmelterItemScript> _smelterItems = new();
        
        private bool _routineActive;
        private bool _smelting;
        private float _time;

        private void Awake()
        {
            foreach (var item in _items)
            {
                var newItem = Instantiate(_smelterItemPrefab, _itemHolder);
                newItem.AssignInfo(
                    item.InputItemName,
                    item.OutputItemName,
                    item.InputItemImage,
                    item.OutputItemImage,
                    item.SmeltTime,
                    item.InputItemType,
                    item.OutputItemType);
                
                Debug.Log(newItem);
                _smelterItems.Add(newItem);
            }
        }

        public void SmeltResouces(string amount, ResourcesScript.Types InputItem, ResourcesScript.Types OutputItem)
        {
            if (!_smelting) return;
            if (!int.TryParse(amount, out var smeltAmount)) return;
            if (GameManager.Instance.ResourcesManagerScript.FindResouce(InputItem).Amount < smeltAmount) return;
            
            GameManager.Instance.ResourcesManagerScript.AffectResource(InputItem, -smeltAmount);
            GameManager.Instance.ResourcesManagerScript.AffectResource(OutputItem, smeltAmount);
            Debug.Log("Smelted Resource");
        }

        public void SmeltButtonVisuals()
        {
            _smeltButtonText.text = "Smelting";
            _smeltButtonBackground.color = _smeltingColor;
        }

        public void StopSmeltingButtonVisuals()
        {
            _smeltButtonText.text = "Smelt";
            _smeltButtonBackground.color = _smeltColor;
        }
    
        public void SmeltButton()
        {
            if (!_smelting)
            {
                foreach (var smelterItem in _smelterItems)
                {
                    smelterItem.Smelt();
                }
            
                _smelting = true;
            }

            else if (_smelting)
            {
                StopSmeltingButtonVisuals();
            
                foreach (var smelterItem in _smelterItems)
                {
                    smelterItem.StopSmelting();
                }
            
                _smelting = false;
            }
        }
    }
}
