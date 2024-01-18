using System;
using Game.Screens;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Manager
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;
        public GameState State { get; private set; } = GameState.Initializing;

        public bool IsInitialized => State != GameState.Initializing;

        void Start() {
            ChangeState(GameState.WaitingInput);
        }

        [Button]
        public void ChangeState(GameState newState) {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState) {
                case GameState.Initializing:
                    break;
                case GameState.WaitingInput:
                    GetSubManager<ScreenManager>().OpenScreen<UpgradeScreen>();
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