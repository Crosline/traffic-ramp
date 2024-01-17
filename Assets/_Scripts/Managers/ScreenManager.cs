using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Screen = Game.Screens.Screen;

namespace Game.Managers
{
    public class ScreenManager : SubManager
    {
        [SerializeField] private List<Screen> _screens;
        [SerializeField] private float _animationDuration = 0.3f;
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
                
                screen.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    OnScreenOpen(screen);
                });

                break;
            }
        }
        public void CloseActiveScreen()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;

            OnScreenClose(screen);
        }
        public void CloseActiveScreenWithAnimation()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;

            screen.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                OnScreenClose(screen);
            });
            

        }

        private void OnScreenOpen(Screen screen)
        {
            screen.OnOpen();
            screen.IsActive = true;
            screen.IsClosing = false;
            OnScreenOpened?.Invoke(screen.Name);
        }
        private void OnScreenClose(Screen screen)
        {
            screen.OnClose();
            screen.IsActive = false;
            OnScreenClosed?.Invoke(screen.Name);
            screen.IsClosing = false;
        }
    }
}