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

        private UpgradeManager _upgradeManger;

        internal override void OnOpen()
        {
            _upgradeManger = (UpgradeManager)UpgradeManager.Instance;

            _rampButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Ramp));
            _fuelButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Fuel));
            _incomeButton.onClick.AddListener(() => OnUpgradeButtonClicked(Upgrades.Income));

            UpdateButtonData();
        }

        internal override void OnClose()
        {
            _rampButton.onClick.RemoveAllListeners();
            _fuelButton.onClick.RemoveAllListeners();
            _incomeButton.onClick.RemoveAllListeners();
        }

        private void UpdateButtonData()
        {
            var rampUpgrade = _upgradeManger.GetUpgrade(Upgrades.Ramp);
            var fuelUpgrade = _upgradeManger.GetUpgrade(Upgrades.Fuel);
            var incomeUpgrade = _upgradeManger.GetUpgrade(Upgrades.Income);

            var coinAmount = ((GameManager)GameManager.Instance).GetCurrentCoins();

            _rampButtonText.text = rampUpgrade.IsMaxedOut ? MaxText : $"{rampUpgrade.Power} m";
            _rampPriceText.text = rampUpgrade.IsMaxedOut ? MaxText :
                rampUpgrade.Price > coinAmount ? "" : $"{rampUpgrade.Price}";

            _fuelButtonText.text = fuelUpgrade.IsMaxedOut ? MaxText : $"{fuelUpgrade.Power} L";
            _fuelPriceText.text = fuelUpgrade.IsMaxedOut ? string.Empty :
                fuelUpgrade.Price > coinAmount ? "" : $"{fuelUpgrade.Price}";

            _incomeButtonText.text = incomeUpgrade.IsMaxedOut ? MaxText : $"{incomeUpgrade.Power} x";
            _incomePriceText.text = incomeUpgrade.IsMaxedOut ? string.Empty :
                incomeUpgrade.Price > coinAmount ? "" : $"{incomeUpgrade.Price}";
        }

        private void OnUpgradeButtonClicked(string upgrade)
        {
            _upgradeManger.Upgrade(upgrade);
            UpdateButtonData();
        }
    }
}