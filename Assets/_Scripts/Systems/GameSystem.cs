using System;
using Game.Utilities;

namespace Game.Systems
{
    [Serializable]
    public abstract class GameSystem : StaticInstance<GameSystem>
    {
        public abstract void Initialize();
        public abstract void Dispose();
    }
}