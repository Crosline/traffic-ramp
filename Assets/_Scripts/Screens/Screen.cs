using UnityEngine;

namespace Game.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        public abstract string Name { get; }
        internal abstract void OnOpen();
        internal abstract void OnClose();
        
        internal bool IsActive { get; set; }
        internal bool IsClosing { get; set; }
    }
}