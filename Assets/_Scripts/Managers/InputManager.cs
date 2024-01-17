using UnityEngine;

namespace Game.Managers
{
    public class InputManager : SubManager
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