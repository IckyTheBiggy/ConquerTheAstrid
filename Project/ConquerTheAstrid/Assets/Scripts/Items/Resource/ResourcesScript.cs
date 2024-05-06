using UnityEngine;

namespace Items.Resource
{
    public class ResourcesScript : MonoBehaviour
    {
        public enum Types
        {
            Money,
            Wood,
            Iron_Ore,
            Gold_Ore,
            Diamond_Ore,
            Stone,
            Iron,
            Gold,
            Diamond
        }

        [System.Serializable]
        public struct PriceItem
        {
            public Resource Resource;
            public int Amount;
        }
    }
}
