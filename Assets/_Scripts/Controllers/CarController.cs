using System;
using DG.Tweening;
using Game.Managers;
using Game.Models.Cars;
using UnityEngine;

namespace Game._Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        private InputManager _inputManager;
        [SerializeField] private float carMinMaxInput = 3.5f;
        [SerializeField] private float smoothNess = 3f;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float pullSpeed = 3f;
        [SerializeField] private float startForce = 20f;
        public bool IsEnabled { get; set; }
        private bool _isShot;
        private bool _isOnRoad;
        [SerializeField]
        private PlayerCar _car;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Rigidbody _rigidbody;

        public CarModel Id => _car.Id;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            var transformTemp = transform;
            initialPosition = transformTemp.position;
            initialRotation = transformTemp.rotation;
            _isShot = false;
            _isOnRoad = false;

            _inputManager = ((GameManager)GameManager.Instance).GetSubManager<InputManager>();
            // InputManager.OnTouchExit += OnPointerRelease;
            // InputManager.OnTouchDeltaMove += OnPointerMove;
        }

        private void OnDestroy()
        {
            // InputManager.OnTouchExit -= OnPointerRelease;
            // InputManager.OnTouchDeltaMove -= OnPointerMove;
        }


        private void Update()
        {
            if (!IsEnabled)
                return;

            if (_isShot) 
                MoveCarHorizontal();

            if (_isOnRoad) 
                MoveCarForward();

            if (_inputManager.TouchUp && !_isShot && !_isOnRoad) 
                OnPointerRelease();
            
            if (!_isShot && !_isOnRoad)
                MoveCarToPull(_inputManager.DeltaMove);
        }

        private void MoveCarToPull(Vector2 deltaMove)
        {
            if (!_inputManager.PointerDown)
                return;

            var vector3 = GetMouseXToCarPosition();

            transform.position = vector3;
            transform.LookAt(initialPosition + initialRotation * Vector3.forward * 2.5f);

            transform.Translate(0f, 0f, deltaMove.y * pullSpeed * Time.deltaTime, Space.World);
            
            
            var zVector3 = transform.position;

            if (zVector3.z > initialPosition.z)
            {
                zVector3.z = initialPosition.z;
            } else if (zVector3.z < initialPosition.z - 4)
            {
                zVector3.z = initialPosition.z - 4;
            }

            transform.position = zVector3;
        }

        private void MoveCarForward()
        {
            var localSpeed = _inputManager.PointerDown ? speed * 1.2f : speed;
            transform.Translate(0f, 0f, localSpeed * Time.deltaTime);
            // _rigidbody.velocity += transform.forward * (localSpeed * Time.deltaTime);
        }

        private void MoveCarHorizontal()
        {
            if (!_inputManager.PointerDown)
                return;

            var vector3 = GetMouseXToCarPosition();

            var posBefore = transform.position.x;
            transform.position = Vector3.Lerp(transform.position, vector3, Time.deltaTime * smoothNess);

            var deltaPos = transform.position.x - posBefore;

            var rotation = initialRotation;
            rotation.z -= deltaPos;
            transform.rotation = rotation;
        }

        private Vector3 GetMouseXToCarPosition()
        {
            var xPos = (_inputManager.MousePosition.x - 0.5f) * carMinMaxInput * 2f;
            var vector3 = transform.position;
            vector3.x = xPos;
            return vector3;
        }

        private void OnPointerRelease()
        {
            var zDistance = initialPosition.z - transform.position.z;
            if(zDistance < 2f)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                
                return;
            }

            _rigidbody.AddForce(transform.forward * (startForce * zDistance), ForceMode.Impulse);
            
            _isShot = true;
            ((GameManager)GameManager.Instance).ChangeState(GameState.Running);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Road"))
            {
                _isOnRoad = true;
                // _rigidbody.isKinematic = true;
                transform.rotation = initialRotation;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ramp"))
            {
                transform.DORotate(initialRotation.eulerAngles, 1f);
            }
        }
    }
}