using System;
using UnityEngine;

namespace Scripts.Model.Data.Properties
{
    [Serializable]
    public class FloatProperty : ObservableProperty<float>
    {
        [SerializeField] private int _capacity;

        public int Capacity
        {
            get => _capacity;
            set => _capacity = value;
        }
    }
}