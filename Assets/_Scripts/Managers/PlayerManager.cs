using System;
using Game._Scripts.Controllers;
using Game.Models.Cars;
using UnityEngine;

namespace Game.Managers
{
    public class PlayerManager : SubManager
    {
        [Header("Player Settings")] 
        [SerializeField]
        private Transform _playerSpawnPoint;

        [SerializeField] private CarController[] _playerCarPrefabs;
        private CarModel SelectedCarId;
        private CarController _spawnedCar;

        public override void Initialize()
        {
            GameManager.OnAfterStateChanged += OnGameStateChanged;

            SelectedCarId = (CarModel)PlayerPrefs.GetInt("PlayerManager_SelectedCarId", 150);
        }

        private CarController GetSelectedCar()
        {
            foreach (var playerCar in _playerCarPrefabs)
                if (playerCar.Id == SelectedCarId)
                    return playerCar;

            throw new ArgumentOutOfRangeException();
        }

        private void OnGameStateChanged(GameState obj)
        {
            if (obj == GameState.WaitingInput)
            {
                _spawnedCar = Instantiate(GetSelectedCar(), _playerSpawnPoint.position, _playerSpawnPoint.rotation, null);
                _spawnedCar.IsEnabled = true;
            }

            if (obj == GameState.Running)
            {
            }

            if (obj == GameState.Lose)
            {
                _spawnedCar.IsEnabled = false;
            }
        }

        public override void Dispose()
        {
            GameManager.OnAfterStateChanged -= OnGameStateChanged;
        }
    }
}