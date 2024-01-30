using System;
using System.Collections.Generic;
using Game.Managers;
using Game.Models.Cars;
using Game.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Game._Scripts.Controllers
{
    public class RoadController : SubManager
    {
        private Transform _player;
        [Header("Car Settings")] 
        [SerializeField]
        private Transform _carParent;
        [SerializeField] private int _carCount;
        [SerializeField] private Car[] _carData;
        [SerializeField] private LayerMask _carLayer;
        private CarObject[] _cars;
        private ObjectPool<GameObject> _carPool;
        private Collider[] _carCollideBuffer;

        [Header("Road Settings")] [SerializeField]
        private Transform _roadParent;

        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private int _roadWidth = 9;
        [SerializeField] private int _roadLength = 5;
        [SerializeField] private LayerMask _roadLayer;
        private static Vector2 _roadSize = new Vector2(3f, 100f);
        private ObjectPool<GameObject> _roadPool;
        private Transform[] _roads;

        private int _roadSpawnIndexer = 0;

        public override void Initialize()
        {
            if (_carParent == null)
                _carParent = new GameObject("Game_CarParent").transform;
            if (_roadParent == null)
                _roadParent = new GameObject("Game_RoadParent").transform;

            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _carCollideBuffer = new Collider[1];
            _cars = new CarObject[_carCount];
            _roads = new Transform[_roadWidth * (_roadLength - _roadSpawnIndexer)];

            _carPool = new ObjectPool<GameObject>(
                OnCreateCar,
                OnGetObject,
                OnReleaseObject,
                OnDestroyObject,
                true, _carCount, _carCount * 2);

            _roadPool = new ObjectPool<GameObject>(
                OnCreateRoad,
                OnGetObject,
                OnReleaseObject,
                OnDestroyObject,
                true, _roadWidth * _roadLength, _roadWidth * _roadLength * 2);

            SpawnRoads();
            
            GameManager.OnAfterStateChanged += OnGameStateChanged;
        }

        private void SetRoadPositionsInitial()
        {
            // can be made to -1 too
            var roadIndex = 0;
            for (int j = _roadSpawnIndexer; j < _roadLength; j++)
            {
                for (int i = -_roadWidth / 2; i < _roadWidth / 2 + 1; i++)
                {
                    var spawnPoint = new Vector3(i * _roadSize.x, 0, j * _roadSize.y);
                    _roads[roadIndex++].transform.localPosition = spawnPoint;
                }
            }
        }

        private void SpawnRoads()
        {
            // can be made to -1 too
            var roadIndex = 0;
            for (int j = _roadSpawnIndexer; j < _roadLength; j++)
            {
                for (int i = -_roadWidth / 2; i < _roadWidth / 2 + 1; i++)
                {
                    var spawnPoint = new Vector3(i * _roadSize.x, 0, j * _roadSize.y);
                    var road = _roadPool.Get();
                    road.transform.localPosition = spawnPoint;
                    _roads[roadIndex++] = road.transform;
                }
            }

        }

        private void SetCarPositionsInitial()
        {
            for (int i = 0; i < _carCount; i++)
            {
                var spawnPoint = GetCarSpawnPoint(0);
                _cars[i].transform.position = spawnPoint;
            }
        }


        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Running)
            {
                SpawnCarsInitial();
                SetCarPositionsInitial();
            }
            if (gameState == GameState.Restart)
            {
                SpawnRoads();
                SetCarPositionsInitial();
                SetRoadPositionsInitial();
            }
        }

        private void SpawnCarsInitial()
        {
            for (int i = 0; i < _carCount; i++)
            {
                var spawnPoint = GetCarSpawnPoint(0);

                var carData = _carData.Rand();
                var car = _carPool.Get();
                car.transform.position = spawnPoint;
                var carObject = new CarObject(car.transform, carData.BaseSpeed);
                _cars[i] = carObject;
            }
        }

        private Vector3 GetCarSpawnPoint(int lengthIncrement)
        {
            Vector3 spawnPoint;

            while (true)
            {
                var randX = Random.Range((float)-_roadWidth / 2, (float)_roadWidth / 2);
                var randY = Random.Range(-_roadSpawnIndexer + 0.15f, _roadLength) + (lengthIncrement * _roadLength);

                spawnPoint = new Vector3(randX * _roadSize.x, 0, randY * _roadSize.y);

                var overlappedCarsLength = Physics.OverlapBoxNonAlloc(spawnPoint, Vector3.one, _carCollideBuffer,
                    Quaternion.identity, _carLayer);
                if (overlappedCarsLength == 0)
                    break;
            }

            return spawnPoint;
        }

        private void Update()
        {
            if (_cars == null)
                return;

            var deltaTime = Time.deltaTime;
            var vec3Forward = Vector3.forward;
            for (int i = 0; i < _carCount; i++)
            {
                var car = _cars[i];
                if (car == null)
                    continue;

                car.transform.position += vec3Forward * (car.speed * deltaTime);

                if (car.transform.position.z < _player.position.z - 5)
                {
                    car.roadIncrement++;
                    car.transform.position = GetCarSpawnPoint(car.roadIncrement);
                }
            }

            foreach (var r in _roads)
            {
                if (r.position.z < _player.position.z - _roadSize.y - 10)
                {
                    var tempPos = r.position;
                    tempPos.z += (_roadLength - _roadSpawnIndexer) * _roadSize.y;

                    r.position = tempPos;
                }
            }
        }


        public override void Dispose()
        {
            _carPool.Clear();
            _carPool.Dispose();
            Destroy(_carParent);

            _roadPool.Clear();
            _roadPool.Dispose();
            Destroy(_roadParent);

            GameManager.OnAfterStateChanged -= OnGameStateChanged;
        }

        private GameObject OnCreateCar()
        {
            Car carData = _carData.Rand();
            GameObject car = Instantiate(carData.Prefab, _carParent, true);
            car.transform.position = Vector3.zero;
            car.SetActive(false);
            car.SetLayersRecursively(_carLayer);
            return car;
        }

        private GameObject OnCreateRoad()
        {
            GameObject road = Instantiate(_roadPrefab, _roadParent, true);
            road.transform.position = Vector3.zero;
            road.SetActive(false);
            road.SetLayersRecursively(_roadLayer);
            return road;
        }

        private void OnGetObject(GameObject obj) => obj.SetActive(true);

        private void OnReleaseObject(GameObject obj) => obj.SetActive(false);

        private void OnDestroyObject(GameObject obj) => Destroy(obj);

        internal class CarObject
        {
            public Transform transform;
            public float speed;
            public int roadIncrement;

            public CarObject(Transform transform, float speed)
            {
                this.transform = transform;
                this.speed = speed;
                roadIncrement = 0;
            }
        }
    }
}