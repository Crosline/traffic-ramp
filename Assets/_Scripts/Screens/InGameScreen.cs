using Game._Scripts.Controllers;
using TMPro;
using UnityEngine;

namespace Game.Screens
{
    public class InGameScreen : Screen
    {
        public override string Name => nameof(InGameScreen);

        private int _distance = 0;
        [SerializeField]
        private TextMeshProUGUI _distanceTMP;

        private CarController _car;

        internal override void OnOpen()
        {
            SetText(0);
            _car = GameObject.FindObjectOfType<CarController>();
        }

        private void Update()
        {
            var distanceTraveled = _car.distanceTraveled;
            if (distanceTraveled == _distance) return;

            _distance = distanceTraveled;

            SetText(_distance);
        }

        internal override void OnClose()
        {
        }

        private void SetText(int distance)
        {
            _distanceTMP.text = $"{distance} m";
        }
    }
}