using Core;
using Items.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class HotbarItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourceAmount;
        [SerializeField] private Image _resourceImage;

        private ResourceSO _resourceSO;

        public void AssignInfo(ResourceSO resourceSO, Sprite resourceImage)
        {
            _resourceSO = resourceSO;
            _resourceImage.sprite = resourceImage;
            _resourceAmount.text =
                GameManager.Instance.ResourcesManagerScript.FindResouce(_resourceSO.Type).Amount.ToString();
        }

        public void UpdateInfo()
        {
            _resourceAmount.text =
                GameManager.Instance.ResourcesManagerScript.FindResouce(_resourceSO.Type).Amount.ToString();
        }
    }
}