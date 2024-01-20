using UnityEngine;

namespace Game.Models.Cars
{
    [CreateAssetMenu(fileName = "Player_Car", menuName = "Game/Car/Player")]
    public class PlayerCar : Car
    {
        [field: SerializeField]
        public float Smoothness { get; private set; }
    }
}