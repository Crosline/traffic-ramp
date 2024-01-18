using System.Collections.Generic;
using Crosline.DataStructures;
using Game.Models;
using UnityEngine;

namespace Game.Managers
{
    public class UpgradeManager : SubManager
    {
        [SerializeField]
        private SerializedDictionary<Upgrade, List<MultiplierData>> _upgrades;
        
        public override void Initialize()
        {
        }

        public override void Dispose()
        {
        }
    }
}