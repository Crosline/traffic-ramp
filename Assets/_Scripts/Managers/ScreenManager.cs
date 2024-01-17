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

        public void OpenScreen(string screenName)
        {
            foreach (var screen in _screens)
            {
                if (!screenName.Equals(screen.name)) continue;

                screen.OnOpen();
                OnScreenOpened?.Invoke(screenName);
                break;
            }
        }
        public void CloseActiveScreen()
        {
            var activeScreen = _activeScreens.Pop();
            
            activeScreen.OnClose();
            OnScreenClosed?.Invoke(activeScreen.name);
        }
        public void CloseActiveScreenWithAnimation()
        {
            
        }
    }
}