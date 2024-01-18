using UnityEngine;

namespace Game.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        // private Canvas _canvas;
        // private void Awake()
        // {
        //     _canvas = GetComponent<Canvas>();
        // }
        //
        // internal void Enable()
        // {
        //     _canvas.enabled = true;
        // }
        //
        // internal void Disable()
        // {
        //     _canvas.enabled = false;
        // }

        public abstract string Name { get; }
        internal abstract void OnOpen();
        internal abstract void OnClose();
        internal bool IsClosing { get; set; } = true;
    }
}