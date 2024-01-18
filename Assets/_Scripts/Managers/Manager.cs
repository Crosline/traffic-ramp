using System.Collections.Generic;
using Game.Utilities;

namespace Game.Managers
{
    public class Manager : Singleton<Manager>
    {
        private List<SubManager> _gameSystems;

        public T GetSubManager<T>() where T : SubManager
        {
            foreach (var subManager in _gameSystems)
                if (subManager is T castedSubManager)
                    return castedSubManager;

            return null;
        }

        protected override void Awake()
        {
            base.Awake();

            _gameSystems = new List<SubManager>();
            
            foreach (var gameSystem in GetComponentsInChildren<SubManager>())
            {
                gameSystem.Initialize();
                _gameSystems.Add(gameSystem);
            }

        }
        
        protected virtual void OnDestroy()
        {
            foreach (var gameSystem in _gameSystems) gameSystem.Dispose();
            _gameSystems.Clear();
        }
    }
}