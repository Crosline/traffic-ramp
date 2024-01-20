using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Managers
{
    public class InputManager : SubManager, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Both X and Y are between 0 and 1
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        private bool _pointerDown;

        public static event Action<Vector2> OnTouchEnter; 
        public static event Action<Vector2> OnTouchExit; 

        public override void Initialize()
        {
            _pointerDown = false;
            MousePosition = Vector2.zero;
        }

        public override void Dispose()
        {
            _pointerDown = false;
        }

        private void Update()
        {
            if (!_pointerDown) return;

            SetMousePos();
        }

        private void SetMousePos()
        {
            if (Input.touchCount == 0) return;

            var pos = Input.touches[0].position;
            MousePosition = new Vector2(pos.x/Screen.width, pos.y/Screen.height);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetMousePos();
            _pointerDown = true;
            OnTouchEnter?.Invoke(MousePosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerDown = false;
            OnTouchExit?.Invoke(MousePosition);
        }
    }
}