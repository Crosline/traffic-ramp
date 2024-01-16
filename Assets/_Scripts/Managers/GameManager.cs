using System;
using System.Collections.Generic;
using Game.Systems;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;
        public GameState State { get; private set; } = GameState.Initializing;

        public bool IsInitialized => State != GameState.Initializing;

        [SerializeField] private List<GameSystem> _gameSystems;

        private void Reset()
        {
            State = GameState.Initializing;
            _gameSystems = new List<GameSystem>();

            foreach (var gameSystem in FindObjectsOfType<GameSystem>())
            {
                if (!_gameSystems.Contains(gameSystem)) _gameSystems.Add(gameSystem);
            }
        }

        private void OnDestroy()
        {
            foreach (var gameSystem in _gameSystems) gameSystem.Dispose();
            _gameSystems.Clear();
        }

        void Start() {

            // if (_screenManager == null)
            //     _screenManager = FindObjectOfType<ScreenManager>();
            //
            // _screenManager.Initialize();
            //
            //
            // if (_inputHandler == null)
            //     _inputHandler = FindObjectOfType<InputHandler>();
            //
            // _inputHandler.Initialize();

            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Initialize();
            }


            ChangeState(GameState.Initializing);
        }

        public void ChangeState(GameState newState) {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState) {
                case GameState.Initializing:
                    break;
                case GameState.WaitingInput:
                    break;
                case GameState.Running:
                    break;
                case GameState.Win:
                    break;
                case GameState.Lose:
                    break;
                case GameState.Restart:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);

            Debug.Log($"New State: {newState}");
        }
    }
    
    
    public enum GameState : byte {
        Initializing = 0,
        WaitingInput = 1,
        Running = 2,
        Win = 3,
        Lose = 4,
        Restart = 5,
        Exit = 255,
    }
}