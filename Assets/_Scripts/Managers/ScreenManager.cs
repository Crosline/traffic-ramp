using Game.Systems;
using UnityEngine;

namespace Game.Managers
{
    public class ScreenManager : GameSystem
    {
        public override void Initialize()
        {
            Debug.Log("YE1");
        }

        public override void Dispose()
        {
            Debug.Log("YE2");
        }
    }
}