using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using Planet;
using UnityEngine;

namespace UserInterface.PlacementMenu
{
    public class PlacementMenuScript : MonoBehaviour
    {
        [SerializeField] private RectTransform _menuHolder;
        [SerializeField] private Transform _plantsMenu, _buildingsMenu;
        [SerializeField] private PlaceableItemScript _itemPrefab;

        private PlanetScript _loadedPlanet;

        private void OnEnable()
        {
            if (_loadedPlanet != GameManager.Instance.PlanetManagerScript.CurrentPlanet) LoadPlanetItems();
        }

        private void LoadPlanetItems()
        {
            _loadedPlanet = GameManager.Instance.PlanetManagerScript.CurrentPlanet;
            foreach (var plant in _loadedPlanet.PlaceablePlants)
            {
                var np = Instantiate(_itemPrefab, _plantsMenu);
                np.Init(plant);
            }

            foreach (var building in _loadedPlanet.PlaceableBuildings)
            {
                var nb = Instantiate(_itemPrefab, _buildingsMenu);
                nb.Init(building);
            }
        }

        #region MenuSwitch

        public void SwitchMenu(int index)
        {
            var targetPos = -index * 1200;
            Misc.RestartCoroutine(this, ref _switchMenuRoutine, SwitchMenuRoutine(targetPos));
        }

        public void CloseUI()
        {
            GameManager.Instance.UIManager.HidePlacementUI();
        }

        private Coroutine _switchMenuRoutine;

        private IEnumerator SwitchMenuRoutine(int targetX)
        {
            float lerpPos = 0;
            var startPos = _menuHolder.anchoredPosition;
            Vector2 targetPos = new(targetX, startPos.y);
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, .5f, false, Easings.Types.ExpoInOut);
                _menuHolder.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            _switchMenuRoutine = null;
        }

        #endregion
    }
}