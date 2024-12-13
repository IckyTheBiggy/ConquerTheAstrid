using System;
using System.Collections.Generic;
using Core;
using Items.Resource;
using UnityEngine;
using UserInterface;

namespace Items.Buildings.Shop
{
    public class ShopUIScript : MenuScript
    {
        [Serializable]
        public struct ShopItemStruct
        {
            public Sprite ItemSprite;
            public string Name;
            public string Description;
            public int Price;
            public int SellPrice;
            public ResourceSO Item;
        }
   
        [SerializeField] private Transform _itemHolder;
        [SerializeField] private ShopItem _shopItemPrefab;
        [SerializeField] private List<ShopItemStruct> _items;

        private void Awake()
        {
            foreach (var item in _items)
            {
                var newItem = Instantiate(_shopItemPrefab, _itemHolder);
                newItem.AssignItemInfo(item.ItemSprite, item.Name, item.Description, item.Price, item.SellPrice, item.Item);
            }
        }

        public void ExitShop() => GameManager.Instance.UIManager.HideMenu();
    }
}
