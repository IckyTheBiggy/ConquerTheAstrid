using Core;
using UnityEngine;

namespace Planet
{
    public class TileScript : MonoBehaviour, IInteractable
    {
        public enum TileTypes { Ground, Liquid }
        
        [Header("Components")]
        private PlanetScript _planetScript;
        private Renderer _renderer;
        
        [Header("Values")]
        [SerializeField] private TileTypes _tileType;
        public TileTypes TileType => _tileType;
        private bool _isSelected;
        public bool IsSelected => _isSelected;
        private Color32 _tileColor;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _tileColor = _renderer.material.color;
            _planetScript = GetComponentInParent<PlanetScript>();
        }

        public void Focus()
        {
            if (IsSelected) return;
            _renderer.material.color = (Color)_tileColor * 1.25f;
            GameManager.Instance.AudioManager.PlaySFX(AudioManager.Sounds.TileFocus, .1f);
        }

        public void Unfocus()
        {
            if (IsSelected) return;
            _renderer.material.color = _tileColor;
        }

        public void Select()
        {
            if (transform.childCount > 0)
            {
                var item = transform.GetChild(0).gameObject;
                GameManager.Instance.SelectionManager.SelectedObject = item;
                return;
            } 
            if (_isSelected) return;
            _renderer.material.color = (Color)_tileColor * 1.5f;
            GameManager.Instance.AudioManager.PlaySFX(AudioManager.Sounds.TileSelect, .1f);
            GameManager.Instance.UIManager.ShowPlacementUI();
            _isSelected = true;
        }

        public void Deselect()
        {
            _renderer.material.color = _tileColor;
            GameManager.Instance.UIManager.HidePlacementUI();
            _isSelected = false;
        }
    }
}
