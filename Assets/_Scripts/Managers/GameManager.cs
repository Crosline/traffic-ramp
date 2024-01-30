using System;
using Game.Screens;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Manager<GameManager>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;
        public GameState State { get; private set; } = GameState.Initializing;

        public bool IsInitialized => State != GameState.Initializing;
        
        public uint GetCurrentCoins() => (uint)PlayerPrefs.GetInt("Player_Coins", 0);

        void Start() {
            ChangeState(GameState.WaitingInput);
        }

        [Button]
        public void ChangeState(GameState newState) {
            if (newState == State) return; // skipping, no need to change state
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState) {
                case GameState.Initializing:
                    GetSubManager<ScreenManager>().CloseAll();
                    break;
                case GameState.WaitingInput:
                    GetSubManager<ScreenManager>().OpenScreen<UpgradeScreen>();
                    break;
                case GameState.Running:
                    GetSubManager<ScreenManager>().CloseAll();
                    GetSubManager<ScreenManager>().OpenScreen<InGameScreen>();
                    break;
                case GameState.Lose:
                    GetSubManager<ScreenManager>().CloseAll();
                    GetSubManager<ScreenManager>().OpenScreen<LoseScreen>();
                    break;
                case GameState.Restart:
                    GetSubManager<ScreenManager>().CloseAll();
                    DOVirtual.DelayedCall(Time.fixedDeltaTime, () => ChangeState(GameState.WaitingInput));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);

            Debug.Log($"New State: {newState}");
        }

        internal void AddCurrency(int income)
        {
            PlayerPrefs.SetInt("Player_Coins", (int)(GetCurrentCoins() + income));
        }
    }
    
    
    public enum GameState : byte {
        Initializing = 0,
        WaitingInput = 1,
        Running = 2,
        Lose = 4,
        Restart = 5,
        Exit = 255,
    }
}