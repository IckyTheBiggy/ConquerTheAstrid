using Core;
using UnityEngine;

namespace Items.Buildings
{
    [CreateAssetMenu (menuName = "Scriptable Objects/New Building", fileName = "Building")]
    public class BuildingSO : ItemSO
    {
        [Header("Building")]
        public float IdlePollution;
        public float RunningPollitoion;
    }
}
