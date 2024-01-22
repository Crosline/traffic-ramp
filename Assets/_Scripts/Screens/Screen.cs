using UnityEngine;

namespace Game.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        protected Canvas _canvas;
        private void OnEnable()
        {
            _canvas ??= GetComponent<Canvas>();
            _canvas.enabled = true;
        }

        public abstract string Name { get; }
        internal abstract void OnOpen();
        internal abstract void OnClose();
        internal bool IsClosing { get; set; } = true;
    }
}