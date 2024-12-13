using System;
using System.Collections.Generic;
using Items.Resource;
using UnityEngine;

namespace UserInterface
{
    public class HotbarScript : MonoBehaviour
    {
        [SerializeField] private HotbarItem _hotbarItemPrefab;
        [SerializeField] private Transform _itemHolder;
        [SerializeField] private List<HotbarResourceStruct> _hotbarResourceStruct;
        private readonly List<HotbarItem> _hotbarItems = new();

        private void Awake()
        {
            foreach (var hotbarItem in _hotbarResourceStruct)
            {
                var newItem = Instantiate(_hotbarItemPrefab, _itemHolder);
                newItem.AssignInfo(hotbarItem.ItemType, hotbarItem.ItemImage);
                _hotbarItems.Add(newItem);
            }
        }

        public void UpdateUI()
        {
            foreach (var hotbarItem in _hotbarItems) hotbarItem.UpdateInfo();
        }

        [Serializable]
        public struct HotbarResourceStruct
        {
            public Sprite ItemImage;
            public ResourceSO ItemType;
        }
    }
}