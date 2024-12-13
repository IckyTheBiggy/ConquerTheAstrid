using System;
using System.Collections.Generic;
using Core;
using Items;
using NnUtils.Scripts;
using UnityEngine;

namespace Planet
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlanetScript : MonoBehaviour, IInteractable
    {
        [Serializable]
        public class PlaceableItem
        {
            public ItemSO ItemSO;
            public int ItemCount;
            public Action<int> OnCountChanged;
            public int MaxItems;
            [Tooltip("Amount the item should move for when switching planets")]
            public float HideOffset;
            public NashSet<GameObject> PlacedItems = new();

            public void PlaceItem(GameObject item)
            {
                ItemCount++;
                PlacedItems.Add(item);
            }

            public void RemoveItem(GameObject item)
            {
                ItemCount--;
                PlacedItems.Remove(item);
            }
        }
        
        [SerializeField] private SphereCollider _planetCollider;
        public float ZoomMin, ZoomMax, ZoomSensitivityMultiplier;
        public List<PlaceableItem> PlaceablePlants;
        public List<PlaceableItem> PlaceableBuildings;
        public readonly Dictionary<ItemSO, PlaceableItem> PlacedItems = new();
        
        private void Reset() => _planetCollider = GetComponent<SphereCollider>();

        private void Awake()
        {
            AddPlaceableItems();
        }

        public void Focus()
        {
            
        }
        
        public void Unfocus()
        {
        }

        public void Select()
        {
            GameManager.Instance.PlanetManagerScript.SwitchPlanet(this);
        }

        public void Deselect()
        {
            
        }

        public void SwitchTo()
        {
            _planetCollider.enabled = false;
            ShowObjects();
        }

        public void SwitchFrom()
        {
            _planetCollider.enabled = true;
            HideObjects();
        }

        public void HideObjects() => MovePlaced(-1);
        public void ShowObjects() => MovePlaced(1);
        private void MovePlaced(int dir)
        {
            foreach (var item in PlacedItems)
            {
                foreach (var placed in item.Value.PlacedItems)
                {
                    placed.transform.position -= item.Value.HideOffset * dir * Vector3.up;
                }
            }
        }

        private void AddPlaceableItems()
        {
            foreach (var plant in PlaceablePlants)
                PlacedItems.Add(plant.ItemSO, plant);
            foreach (var building in PlaceableBuildings)
                PlacedItems.Add(building.ItemSO, building);
        }
        
        public void PlaceItem(ItemSO itemSO)
        {
            var newItem = Instantiate(itemSO.ItemPrefab, GameManager.Instance.SelectionManager.SelectedObject!.transform);
            GameManager.Instance.SelectionManager.SelectedObject = null;
            PlacedItems[itemSO].PlaceItem(newItem);
        }

        public void RemoveItem(ItemSO itemSO, GameObject obj)
        {
            PlacedItems[itemSO].RemoveItem(obj);
            Destroy(obj);
        }
    }
}
