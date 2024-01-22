using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens
{
    public class UpgradeScreen : Screen
    {
        [Header("Buttons")] 
        [SerializeField] private Button _rampButton;
        [SerializeField] private Button _fuelButton;
        [SerializeField] private Button _incomeButton;
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI _rampButtonText;
        [SerializeField] private TextMeshProUGUI _fuelButtonText;
        [SerializeField] private TextMeshProUGUI _incomeButtonText;
        [Header("Prices")] 
        [SerializeField] private TextMeshProUGUI _rampPriceText;
        [SerializeField] private TextMeshProUGUI _fuelPriceText;
        [SerializeField] private TextMeshProUGUI _incomePriceText;

        private const string MaxText = "MAX";

        public override string Name => nameof(UpgradeScreen);

        private UpgradeManager _upgradeManager;

        internal override void OnOpen()
        {
            _upgradeManager = ((GameManager)GameManager.Instance).GetSubManager<UpgradeManager>();

            _rampButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Ramp));
            _fuelButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Fuel));
            _incomeButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Income));

            UpdateButtonData();
            InputManager.OnPointerEntered += OnPointerEntered;
            InputManager.OnPointerExited += OnPointerExited;
        }

        internal override void OnClose()
        {
            _rampButton.onClick.RemoveAllListeners();
            _fuelButton.onClick.RemoveAllListeners();
            _incomeButton.onClick.RemoveAllListeners();
            InputManager.OnPointerEntered -= OnPointerEntered;
            InputManager.OnPointerExited -= OnPointerExited;
        }

        private void UpdateButtonData()
        {
            var rampUpgrade = _upgradeManager.GetUpgrade(Upgrades.Ramp);
            var fuelUpgrade = _upgradeManager.GetUpgrade(Upgrades.Fuel);
            var incomeUpgrade = _upgradeManager.GetUpgrade(Upgrades.Income);

            var coinAmount = ((GameManager)GameManager.Instance).GetCurrentCoins();

            _rampButtonText.text = $"{rampUpgrade.Power} m";
            _rampPriceText.text = rampUpgrade.IsMaxedOut ? MaxText :
                rampUpgrade.Price > coinAmount ? "" : $"{rampUpgrade.Price}";

            _fuelButtonText.text = $"{fuelUpgrade.Power} L";
            _fuelPriceText.text = fuelUpgrade.IsMaxedOut ? MaxText :
                fuelUpgrade.Price > coinAmount ? "" : $"{fuelUpgrade.Price}";

            _incomeButtonText.text = $"{incomeUpgrade.Power} x";
            _incomePriceText.text = incomeUpgrade.IsMaxedOut ? MaxText :
                incomeUpgrade.Price > coinAmount ? "" : $"{incomeUpgrade.Price}";
        }

        private void OnUpgradeButtonClicked(string upgrade)
        {
            _upgradeManager.Upgrade(upgrade);
            UpdateButtonData();
        }

        private void OnPointerExited(Vector2 obj)
        {
            _canvas.enabled = true;
        }

        private void OnPointerEntered(Vector2 obj)
        {
            _canvas.enabled = false;
        }
    }
}