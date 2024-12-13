using System;
using System.Collections.Generic;
using Core;
using Items.Buildings.Factory;
using Items.Resource;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Items.Buildings.Smelter
{
    public class FactoryScript : MonoBehaviour
    {
        [Serializable]
        public struct FactoryItemStruct
        {
            public string InputItemName;
            public string OutputItemName;
            public Sprite InputItemImage;
            public Sprite OutputItemImage;
            public float CraftTime;
            public ResourcesScript.Types InputItemType;
            public ResourcesScript.Types OutputItemType;
        }
        
        [SerializeField] private List<FactoryItemStruct> _items;
        [SerializeField] private FactoryItemScript _factoryItemPrefab;
        [SerializeField] private Transform _itemHolder;
        
        [SerializeField] private ResourceSO _inputItemSO;
        [SerializeField] private ResourceSO _outPutItemSO;
    
        [SerializeField] private float _craftTime;
    
        [SerializeField] private Color32 _craftColor;
        [SerializeField] private Color32 _craftingColor;
    
        [SerializeField] private TMP_Text _craftButtonText;
        [SerializeField] private Image _craftButtonBackground;

        private List<FactoryItemScript> _factoryItems = new();
        
        private bool _routineActive;
        private bool _crafting;
        private float _time;

        private void Awake()
        {
            foreach (var item in _items)
            {
                var newItem = Instantiate(_factoryItemPrefab, _itemHolder);
                newItem.AssignInfo(
                    item.InputItemName,
                    item.OutputItemName,
                    item.InputItemImage,
                    item.OutputItemImage,
                    item.CraftTime,
                    item.InputItemType,
                    item.OutputItemType);
                
                Debug.Log(newItem);
                _factoryItems.Add(newItem);
            }
        }

        public void CraftResouces(string amount, ResourcesScript.Types InputItem, ResourcesScript.Types OutputItem)
        {
            if (!_crafting) return;
            if (!int.TryParse(amount, out var craftAmount)) return;
            if (GameManager.Instance.ResourcesManagerScript.FindResouce(InputItem).Amount < craftAmount) return;
            
            GameManager.Instance.ResourcesManagerScript.AffectResource(InputItem, -craftAmount);
            GameManager.Instance.ResourcesManagerScript.AffectResource(OutputItem, craftAmount);
            Debug.Log("Crafted Resource");
        }

        public void CraftButtonVisuals()
        {
            _craftButtonText.text = "Crafting";
            _craftButtonBackground.color = _craftingColor;
        }

        public void StopCraftingButtonVisuals()
        {
            _craftButtonText.text = "Craft";
            _craftButtonBackground.color = _craftColor;
        }
    
        public void CraftButton()
        {
            if (!_crafting)
            {
                foreach (var factoryItem in _factoryItems)
                {
                    factoryItem.Craft();
                }
            
                _crafting = true;
            }

            else if (_crafting)
            {
                StopCraftingButtonVisuals();
            
                foreach (var factoryItem in _factoryItems)
                {
                    factoryItem.StopCrafting();
                }
            
                _crafting = false;
            }
        }
    }
}
