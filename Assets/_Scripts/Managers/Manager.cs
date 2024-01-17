using System.Collections.Generic;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class Manager : PersistentSingleton<Manager>
    {
        [SerializeField] private List<SubManager> _gameSystems;

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
            
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Initialize();
            }

        }

        protected virtual void Reset()
        {
            _gameSystems = new List<SubManager>();

            foreach (var gameSystem in FindObjectsOfType<SubManager>())
            {
                if (!_gameSystems.Contains(gameSystem)) _gameSystems.Add(gameSystem);
            }
        }
        
        protected virtual void OnDestroy()
        {
            foreach (var gameSystem in _gameSystems) gameSystem.Dispose();
            _gameSystems.Clear();
        }
    }
}