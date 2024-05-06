using Core;
using UnityEngine;

namespace Items.Plants
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Plant", fileName = "New Plant")]
    public class PlantSO : ItemSO
    {
        [Header("Plant")]
        public int Health;
        public float GrowthTime;
		public AnimationCurve GrowthCurve;

    }
}
