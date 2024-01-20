using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Managers
{
    public class InputManager : SubManager, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Both X and Y w
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        private bool _pointerDown;

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

            if (Input.touchCount == 0) return;

            Debug.Log($"Position: {Input.touches[0].position}");
            Debug.Log($"Raw Position: {Input.touches[0].rawPosition}");

            var pos = Input.touches[0].position;
            MousePosition = new Vector2(pos.x/Screen.width, pos.y/Screen.height);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerDown = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerDown = false;
        }
    }
}