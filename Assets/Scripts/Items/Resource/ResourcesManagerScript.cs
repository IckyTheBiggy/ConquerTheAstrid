using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Items.Resource
{
    public class ResourcesManagerScript : MonoBehaviour
    {
        [SerializeField] private List<Resource> _resources;
        public List<Resource> Resources => _resources;
        
        public void AffectResource(ResourcesScript.Types type, int amount)
        {
            Resources.Find(x => x._resourceSO.Type == type).Amount += amount;
            GameManager.Instance.HotbarScript.UpdateUI();
        }

        public Resource FindResouce(ResourcesScript.Types type) => Resources.Find(x => x._resourceSO.Type == type);
    }
}