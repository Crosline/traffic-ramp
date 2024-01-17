using System;
using System.Collections.Generic;
using UnityEngine;
using Screen = Game.Screens.Screen;

namespace Game.Managers
{
    public class ScreenManager : SubManager
    {
        [SerializeField] private List<Screen> _screens;
        public static event Action<string> OnScreenOpened;
        public static event Action<string> OnScreenClosed;
        private Stack<Screen> _activeScreens;
        public override void Initialize()
        {
            _activeScreens = new Stack<Screen>();
        }

        public override void Dispose()
        {
            _activeScreens.Clear();
        }

        public void OpenScreen<T>() where T : Screen
        {
            foreach (var screen in _screens)
            {
                if (screen.IsActive) continue;
                if (screen is not T) continue;

                screen.OnOpen();
                screen.IsActive = true;
                OnScreenOpened?.Invoke(screen.Name);
                break;
            }
        }
        public void CloseActiveScreen()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;
            
            screen.OnClose();
            screen.IsActive = false;
            OnScreenClosed?.Invoke(screen.Name);
            screen.IsClosing = false;
        }
        public void CloseActiveScreenWithAnimation()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;
            
            screen.OnClose();
            screen.IsActive = false;
            OnScreenClosed?.Invoke(screen.Name);
            screen.IsClosing = false;
        }
    }
}