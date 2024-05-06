using System;
using UnityEngine;

namespace Core
{
    public class PollutionScript : MonoBehaviour
    {
        private float _pollutionAmount;
        public float PollutionAmount
        {
            get => _pollutionAmount;
            private set
            {
                if (value <= 0) value = 0;
                _pollutionAmount = value;
                OnPollutionChanged?.Invoke();
            }
        }
        public Action OnPollutionChanged;

        public void AffectPollution(float amount) => PollutionAmount += amount;
    }
}
