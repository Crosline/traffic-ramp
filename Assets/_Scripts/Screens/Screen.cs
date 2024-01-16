using UnityEngine;

namespace Game.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        public abstract void OnOpen();
        public abstract void OnClose();
    }
}