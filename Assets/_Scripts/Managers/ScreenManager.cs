using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Screen = Game.Screens.Screen;

namespace Game.Managers
{
    public class ScreenManager : SubManager
    {
        [SerializeField] private Screen[] _screens;
        [SerializeField] private float _animationDuration = 0.3f;
        public static event Action<string> OnScreenOpened;
        public static event Action<string> OnScreenClosed;
        private Stack<Screen> _activeScreens;

        public Screen ActiveScreen => _activeScreens.Peek();

        public override void Initialize()
        {
            _activeScreens = new Stack<Screen>();
            
            if (_screens == null || _screens.Length == 0)
            {
                _screens = FindObjectsOfType<Screen>();
            }

            foreach (var screen in _screens)
            {
                screen.gameObject.SetActive(false);
            }
        }

        public override void Dispose()
        {
            _activeScreens.Clear();
        }
        
        private T GetScreen<T>() where T : Screen
        {
            foreach (var screen in _screens)
                if (screen is T sc) return sc;

            throw new ArgumentOutOfRangeException("No screen found with type: " + typeof(T).Name);
        }

        public void OpenScreen<T>() where T : Screen
        {
            var screen = GetScreen<T>();
            screen.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                OpenScreenInternal(screen);
            });
        }

        public void OpenScreenNow<T>() where T : Screen
        {
            var screen = GetScreen<T>();
            OpenScreenInternal(screen);
        }

        public void CloseActiveScreenWithoutAnimation()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;

            CloseScreenInternal(screen);
        }

        public void CloseActiveScreen()
        {
            var screen = _activeScreens.Pop();
            screen.IsClosing = true;

            screen.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                CloseScreenInternal(screen);
            });
        }

        private void OpenScreenInternal(Screen screen)
        {
            screen.gameObject.SetActive(true);
            screen.OnOpen();
            screen.IsClosing = false;
            OnScreenOpened?.Invoke(screen.Name);
        }

        private void CloseScreenInternal(Screen screen)
        {
            screen.gameObject.SetActive(false);
            screen.OnClose();
            OnScreenClosed?.Invoke(screen.Name);
            screen.IsClosing = false;
        }
    }
}