using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class Manager<T> : Singleton<Manager<T>> where T : Manager<T>, new()
    {
        [SerializeField] private List<SubManager> _gameSystems;

        public TSubManager GetSubManager<TSubManager>()
        {
            foreach (var subManager in _gameSystems)
                if (subManager is TSubManager castedSubManager)
                    return castedSubManager;

            throw new ArgumentException();
        }

        protected override void Awake()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            base.Awake();

            foreach (var gameSystem in _gameSystems)
                gameSystem.Initialize();
        }

        protected virtual void OnDestroy()
        {
            foreach (var gameSystem in _gameSystems) gameSystem.Dispose();
            _gameSystems.Clear();
        }
    }
}