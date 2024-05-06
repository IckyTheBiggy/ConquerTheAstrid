using System;
using JetBrains.Annotations;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class SelectionManager : MonoBehaviour
    {
        private const int MAX_DISTANCE = 5000;
        private Vector3 _startingMousePos;
        [SerializeField] private LayerMask _selectableMask;
        [SerializeField] private float _pixelThreshold = 10f;
        [CanBeNull] private GameObject _hoveredObject, _selectedObject;
        [CanBeNull]
        public GameObject HoveredObject
        {
            get => _hoveredObject;
            set
            {
                if (_hoveredObject == value) return;
                if (_hoveredObject != null) _hoveredObject.GetComponent<IInteractable>()?.Unfocus();
                _hoveredObject = value;
                if (_hoveredObject != null) _hoveredObject.GetComponent<IInteractable>()?.Focus();
                OnHoveredChanged?.Invoke();
            }
        }
        public Action OnHoveredChanged;

        [CanBeNull] private GameObject _clickedObject;
        
        [CanBeNull]
        public GameObject SelectedObject
        {
            get => _selectedObject;
            set
            {
                _toBeSelected = value;
                if (_selectedObject != null) _selectedObject.GetComponent<IInteractable>()?.Deselect();
                _previouslySelectedObject = _selectedObject;
                _selectedObject = value;
                _toBeSelected = null;
                if (_selectedObject != null) _selectedObject.GetComponent<IInteractable>()?.Select();
                OnSelectedChanged?.Invoke();
            }
        }
        public Action OnSelectedChanged;

        [CanBeNull] private GameObject _previouslySelectedObject;
        [CanBeNull] public GameObject PreviouslySelectedObject => _previouslySelectedObject;
        [CanBeNull] private GameObject _toBeSelected;
        [CanBeNull] public GameObject ToBeSelected => _toBeSelected;
        
        private void Update()
        {
            if (Misc.IsPointerOverUI)
            {
                HoveredObject = null;
                return;
            }
#if UNITY_STANDALONE
            StandaloneSelection();
#endif
#if UNITY_ANDROID
            MobileSelection();
#endif
        }

        private void StandaloneSelection()
        {
            var hovered = GetHoveredObject();
            HoveredObject = hovered;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _startingMousePos = Input.mousePosition;
                _clickedObject = HoveredObject;
            }
            if (!Input.GetKeyUp(KeyCode.Mouse0)) return;
            if (Vector3.Distance(_startingMousePos, Input.mousePosition) <= _pixelThreshold && _clickedObject == HoveredObject)
                SelectedObject = HoveredObject;
            _startingMousePos = Vector3.zero;
        }

        private void MobileSelection()
        {
            if (Input.touchCount != 1) return;
            var touch = Input.GetTouch(0);
            var ray = GameManager.Instance.MainCam.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out var hit, MAX_DISTANCE, _selectableMask);
            if (touch.phase == TouchPhase.Began) _clickedObject = hit.transform.gameObject;
            if (touch.phase != TouchPhase.Ended) return;
            if (touch.deltaPosition.magnitude > _pixelThreshold) return;
            if (_clickedObject != HoveredObject) return;
            SelectedObject = hit.transform.gameObject;
        }
        
        [CanBeNull] private GameObject GetHoveredObject() =>
            Physics.Raycast(
                GameManager.Instance.MainCam.ScreenPointToRay(Input.mousePosition),
                out var hit,
                MAX_DISTANCE,
                _selectableMask)
                ? hit.transform.gameObject
                : null;
    }
}