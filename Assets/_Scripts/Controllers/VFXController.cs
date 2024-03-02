using System;
using DG.Tweening;
using Game.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class VFXController : PersistentSingleton<VFXController>
    {
        [SerializeField] private GameObject _gearVFXPrefab;
        private ObjectPool<GameObject> _gearPool;

        [SerializeField] private GameObject _smokeVFXPrefab;
        private ObjectPool<GameObject> _smokePool;

        protected override void Awake()
        {
            base.Awake();

            _gearPool = new ObjectPool<GameObject>(
                OnCreateGear,
                OnGetGameObject,
                OnReleaseGameObject,
                OnDestroyGameObject,
                true, 5, 20);

            _smokePool = new ObjectPool<GameObject>(
                OnCreateSmoke,
                OnGetGameObject,
                OnReleaseGameObject,
                OnDestroyGameObject,
                true, 1, 3);
        }

        #region Generic

        private void OnDestroyGameObject(GameObject obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnReleaseGameObject(GameObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnGetGameObject(GameObject obj)
        {
            obj.gameObject.SetActive(true);
        }

        private GameObject OnCreateGear()
        {
            GameObject instantiate = Instantiate(_gearVFXPrefab, null, true);
            instantiate.gameObject.SetActive(false);
            return instantiate;
        }

        private GameObject OnCreateSmoke()
        {
            GameObject instantiate = Instantiate(_smokeVFXPrefab, null, true);
            instantiate.gameObject.SetActive(false);
            return instantiate;
        }

        #endregion

        public void SpawnVFX(Vector3 position, GenericVFXType type)
        {
            switch (type)
            {
                case GenericVFXType.Gear:
                    SpawnGenericVFX(position, _gearPool, 1.5f);
                    break;
                case GenericVFXType.Smoke:
                    SpawnGenericVFX(position, _smokePool, 10f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SpawnGenericVFX(Vector3 position, ObjectPool<GameObject> objPool, float delay = 1f)
        {
            GameObject tempVFX = objPool.Get();
            tempVFX.transform.position = position;
            DOVirtual.DelayedCall(delay, () => objPool.Release(tempVFX));
        }
    }

    public enum GenericVFXType
    {
        Gear,
        Smoke
    }
}