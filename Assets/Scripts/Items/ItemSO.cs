using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item", fileName = "New Item")]
    public class ItemSO : ScriptableObject
    {
        [Header("Item")]
        public string ItemName;
        public string ItemDescription;
        public Sprite ItemSprite;
        public ItemTypes ItemType;
        public GameObject ItemPrefab;
    }
}