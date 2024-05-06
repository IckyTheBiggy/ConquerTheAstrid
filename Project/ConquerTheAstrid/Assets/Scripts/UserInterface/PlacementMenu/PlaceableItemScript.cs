using Core;
using Items;
using Planet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.PlacementMenu
{
    public class PlaceableItemScript : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameText, _descText;
        [SerializeField] private TMP_Text _countText, _leftText;
        [SerializeField] private Color32 _placementLimitReachedColor;
        private int _count, _left;
        private PlanetScript.PlaceableItem _item;
        private ItemSO _itemSO;

        public void Init(PlanetScript.PlaceableItem item)
        {
            _item = item;
            _itemSO = item.ItemSO;
            _image.sprite = _itemSO.ItemSprite;
            _nameText.text = _itemSO.ItemName;
            _descText.text = _itemSO.ItemDescription;
            UpdateCount(item.PlacedItems.Count);
            item.PlacedItems.OnCountChanged += UpdateCount;
            UpdateLeft(item.ItemCount);
            item.OnCountChanged += UpdateLeft;
        }

        private void UpdateCount(int itemCount)
        {
            _countText.text = $"Placed: {itemCount}/{_item.MaxItems}";
        }

        private void UpdateLeft(int itemsLeft)
        {
            _leftText.text = $"Left: {itemsLeft}";
        }

        public void PlaceItem()
        {
            if (_item.PlacedItems.Count >= _item.MaxItems) return;
            GameManager.Instance.PlanetManagerScript.CurrentPlanet.PlaceItem(_itemSO);
        }
    }
}