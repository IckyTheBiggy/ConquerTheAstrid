using Core;
using Items.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Buildings.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemDescription;
        [SerializeField] private TMP_Text _itemPrice;
        [SerializeField] private TMP_Text _itemSellPrice;
        [SerializeField] private ResourceSO _item;

        private int _price;
        private int _sellPrice;
    
        public void AssignItemInfo(
            Sprite itemSprite,
            string itemName,
            string itemDescription,
            int itemPrice,
            int itemSellPrice,
            ResourceSO item)
        {
            _itemImage.sprite = itemSprite;
            _itemName.text = itemName;
            _itemDescription.text = itemDescription;
            _itemPrice.text = $"Buy: ${itemPrice}";
            _itemSellPrice.text = $"Sell: ${itemSellPrice}";
            _price = itemPrice;
            _sellPrice = itemSellPrice;
            _item = item;
        }

        public void Buy()
        {
            if (GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Money).Amount >= _price)
            {
            GameManager.Instance.ResourcesManagerScript.AffectResource(ResourcesScript.Types.Money, -_price);
            GameManager.Instance.ResourcesManagerScript.AffectResource(_item.Type, 1);
            }
        }

        public void Sell()
        {
            if (GameManager.Instance.ResourcesManagerScript.FindResouce(_item.Type).Amount >= 1)
            {
                GameManager.Instance.ResourcesManagerScript.AffectResource(ResourcesScript.Types.Money, _sellPrice);
                GameManager.Instance.ResourcesManagerScript.AffectResource(_item.Type, -1);
            }
        }
    }
}
