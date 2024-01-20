using UnityEngine;

namespace Game.Models.Cars
{
    [CreateAssetMenu(fileName = "Car", menuName = "Game/Car/Default")]
    public class Car : ScriptableObject
    {
        [field: SerializeField]
        public CarModel Id { get; private set; }
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        [field: SerializeField]
        public float BaseSpeed { get; private set; }
    }


    public enum CarModel : byte
    {
        Sedan = 100,
        SedanSports = 105,
        Racer = 110,
        RacerFuturistic = 150,
    }
}