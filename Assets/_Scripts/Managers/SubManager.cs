using System;
using Game.Utilities;

namespace Game.Managers
{
    [Serializable]
    public abstract class SubManager : StaticInstance<SubManager>
    {
        public abstract void Initialize();
        public abstract void Dispose();
    }
}