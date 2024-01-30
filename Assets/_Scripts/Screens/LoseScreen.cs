using Game._Scripts.Controllers;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens
{
    public class LoseScreen : Screen
    {
        public override string Name => nameof(LoseScreen);

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private TextMeshProUGUI _incomeText;

        [SerializeField]
        private TextMeshProUGUI _distanceText;

        private int income;

        internal override void OnOpen()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            var distance = FindObjectOfType<CarController>().distanceTraveled;

            income = distance * (int)((GameManager)GameManager.Instance).GetSubManager<UpgradeManager>().GetUpgrade(Upgrades.Income).Power;

            _incomeText.text = income.ToString();
            _distanceText.text = distance.ToString();
        }

        internal override void OnClose()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        public void OnRestartButtonClicked()
        {
            var gm = ((GameManager)GameManager.Instance);
            gm.AddCurrency(income);
            gm.ChangeState(GameState.Restart);
        }
    }
}