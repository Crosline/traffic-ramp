using Crosline.DataStructures;
using Game.Models;
using UnityEngine;

namespace Game.Managers
{
    public class UpgradeManager : SubManager
    {
        [SerializeField]
        private SerializedDictionary<string, Upgrade> _upgrades;
        
        public override void Initialize()
        {
            foreach (var upgrade in _upgrades.Values)
            {
                upgrade.Initialize();
            }
        }

        public override void Dispose()
        {
            _upgrades.Clear();
        }

        public void Upgrade(string upgrade)
        {
            _upgrades[upgrade].IncreasePower();
        }
    }

    public class Upgrades
    {
        public const string Fuel = nameof(Fuel);
        public const string Income = nameof(Income);
        public const string Ramp = nameof(Ramp);
    }
}