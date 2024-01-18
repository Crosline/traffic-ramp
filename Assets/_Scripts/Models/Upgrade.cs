using System;
using UnityEngine;

namespace Game.Models
{
    [Serializable]
    public class Upgrade
    {
        [SerializeField]
        private string name;
    }

    [Serializable]
    public struct MultiplierData
    {
        public float Multiplier;
        public int Count;
    }
}