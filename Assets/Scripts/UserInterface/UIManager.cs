using System.Collections.Generic;
using Items.Plants;
using UnityEngine;

namespace UserInterface
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform _menuParent;

        [SerializeField] private Transform _infoParent;

        [SerializeField] private GameObject _hotbar;

        [SerializeField] private GameObject _placementMenu;

        [Header("Info Scripts")] [SerializeField]
        private PlantInfoScript _plantInfo;

        private readonly Dictionary<GameObject, MenuScript> _objectMenuPairs = new();
        public MenuScript ActiveMenu { get; private set; }

        public InfoScript ActiveInfo { get; private set; }

        public void ShowInfo(object info)
        {
            if (ActiveInfo != null && ActiveInfo.InfoObject == info) return;
            HideMenu();
            HideInfo();
            HidePlacementUI();
            _infoParent.gameObject.SetActive(true);
            switch (info)
            {
                case PlantScript script:
                    ActiveInfo = ShowInfo(script);
                    break;
            }
        }

        private InfoScript ShowInfo(PlantScript ps)
        {
            _plantInfo.Show(ps);
            return _plantInfo;
        }

        public void HideInfo()
        {
            if (ActiveInfo == null) return;
            ActiveInfo.Hide();
            ActiveInfo = null;
            _infoParent.gameObject.SetActive(false);
        }

        public void ShowMenu(MenuScript menu, GameObject obj)
        {
            if (!_objectMenuPairs.ContainsKey(obj)) _objectMenuPairs.Add(obj, Instantiate(menu, _menuParent));
            if (ActiveMenu == _objectMenuPairs[obj]) return;
            HideMenu();
            HideInfo();
            HidePlacementUI();
            _hotbar.SetActive(false);
            ActiveMenu = _objectMenuPairs[obj];
            _menuParent.gameObject.SetActive(true);
            ActiveMenu.Show();
        }

        public void HideMenu()
        {
            if (ActiveMenu == null) return;
            ActiveMenu.Hide();
            _hotbar.SetActive(true);
            ActiveMenu = null;
            _menuParent.gameObject.SetActive(false);
        }

        public void ShowPlacementUI()
        {
            _placementMenu.SetActive(true);
            HideMenu();
            HideInfo();
            _hotbar.SetActive(false);
        }

        public void HidePlacementUI()
        {
            _placementMenu.SetActive(false);
            _hotbar.SetActive(true);
        }
    }
}