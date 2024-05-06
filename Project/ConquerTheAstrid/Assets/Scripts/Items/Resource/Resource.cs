using System;

namespace Items.Resource
{
    [System.Serializable]
    public class Resource
    {
        public ResourceSO _resourceSO;
        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                if (_amount == value) return;
                if (value < 0) value = 0;
                _amount = value;
                OnAmountChanged?.Invoke();
            }
        }
        public Action OnAmountChanged;
    }
}