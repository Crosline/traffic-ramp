using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class Upgrade
    {
        protected virtual string Name { get; }
        [SerializeField]
        private List<float> _powersByLevel;

        private int _multiplierIndex;

        public float Power { get; private set; }
        public bool IsMaxedOut => _multiplierIndex == _powersByLevel.Count - 1;

        public virtual void Initialize()
        {
            _multiplierIndex = PlayerPrefs.GetInt($"Upgrade_{Name}_index", 0);
            SettlePower();
        }

        public virtual void IncreasePower()
        {
            if (IsMaxedOut) return;

            _multiplierIndex++;
            SettlePower();
            PlayerPrefs.SetInt($"Upgrade_{Name}_index", _multiplierIndex);
        }

        private void SettlePower()
        {
            Power = _powersByLevel[_multiplierIndex];
        }

        ~Upgrade() => PlayerPrefs.SetInt($"Upgrade_{Name}_index", _multiplierIndex);
    }

    [Serializable]
    public struct MultiplierData
    {
        [field: SerializeField]
        public float NewPower { get; private set; }
        [field: SerializeField]
        public int Count { get; private set; }
    }
}