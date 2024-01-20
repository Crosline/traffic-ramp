using Game.Managers;
using Game.Models.Cars;
using Game.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Game._Scripts.Controllers
{
    public class RoadController : SubManager
    {
        [Header("Car Settings")]
        [SerializeField] private Transform _carParent;
        [SerializeField] private int _carCount;
        [SerializeField] private Car[] _carData;
        [SerializeField] private LayerMask _carLayer;
        private Transform[] _cars;
        private ObjectPool<GameObject> _carPool;
        [Header("Road Settings")]
        [SerializeField] private Transform _roadParent;
        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private int _roadWidth;
        [SerializeField] private LayerMask _roadLayer;
        private static Vector2 _roadSize = new Vector2(3f, 100f);
        private ObjectPool<GameObject> _roadPool;
        private int _roadPlacerAmount = 3;

        private Car GetRandomCarData() => _carData.Rand();

        public override void Initialize()
        {
            if (_carParent == null)
                _carParent = new GameObject("Game_CarParent").transform;
            if (_roadParent == null)
                _roadParent = new GameObject("Game_RoadParent").transform;
            
            _carPool = new ObjectPool<GameObject>(
                OnCreateCar,
                OnGetObject,
                OnReleaseObject,
                OnDestroyObject,
                true, _carCount , _carCount * 2);
            
            _roadPool = new ObjectPool<GameObject>(
                OnCreateRoad,
                OnGetObject,
                OnReleaseObject,
                OnDestroyObject,
                true, _roadWidth * _roadPlacerAmount , _roadWidth * _roadPlacerAmount * 2);

            SpawnRoads();
        }

        private void SpawnRoads()
        {
            for (int j = -1; j < _roadPlacerAmount; j++)
            {
                for (int i = -_roadWidth/2; i < _roadWidth/2 + 1; i++)
                {
                        var spawnPoint = new Vector3(i * _roadSize.x, 0, j * _roadSize.y);
                        var road = _roadPool.Get();
                        road.transform.localPosition = spawnPoint;
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
        }

        private GameObject OnCreateCar()
        {
            Car carData = GetRandomCarData();
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
    }
}