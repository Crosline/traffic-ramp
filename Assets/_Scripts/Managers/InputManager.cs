﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Managers
{
    public class InputManager : SubManager, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        /// <summary>
        /// Both X and Y are between 0 and 1
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        private Vector2 _mousePositionLastFrame;
        public Vector2 DeltaMove { get; private set; }

        public static event Action<Vector2> OnPointerEntered;
        public static event Action<Vector2> OnPointerExited;

        private bool _pointerDown;

        public bool PointerDown => Input.touchCount > 0 && _pointerDown;

        private bool _pointerStatusLastFrame = false;
        public bool TouchUp => _pointerStatusLastFrame && !PointerDown;

        public override void Initialize()
        {
            _pointerDown = false;
            MousePosition = Vector2.zero;
            _mousePositionLastFrame = Vector2.zero;
        }

        public override void Dispose()
        {
            _pointerDown = false;
        }

        private void Update()
        {
            if (!PointerDown) return;

            SetMousePos();
        }

        private void LateUpdate()
        {
            _pointerStatusLastFrame = PointerDown;
            _mousePositionLastFrame = MousePosition;
        }

        private void SetMousePos()
        {
            if (Input.touchCount == 0) return;

            var pos = Input.touches[0].position;
            MousePosition = new Vector2(pos.x/Screen.width, pos.y/Screen.height);
            DeltaMove = MousePosition - _mousePositionLastFrame;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetMousePos();
            _mousePositionLastFrame = MousePosition;
            DeltaMove = Vector2.zero;
            _pointerDown = true;
            OnPointerEntered?.Invoke(MousePosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerDown = false;
            OnPointerExited?.Invoke(MousePosition);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
        }
    }
}