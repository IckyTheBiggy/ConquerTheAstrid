using Core;
using UnityEngine;

namespace Items.Resource
{
    [CreateAssetMenu(menuName = "Scriptable Objects/New Resource", fileName = "Resource")]
    public class ResourceSO : ItemSO
    {
        [Header("Resource")]
        public ResourcesScript.Types Type;
    }
}
