using TMPro;
using UnityEngine;

namespace SlotMachine
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private SlotMachineController _slotMachineController;

        [SerializeField] private TextMeshProUGUI _moneyBalanceText;
        [SerializeField] private TextMeshProUGUI _moneyWonText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private Transform _handlePointerInteractDisplay;
        public static UIManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Update()
        {
            if (_slotMachineController.SlotMachineState == SlotMachineState.READY)
                _handlePointerInteractDisplay.localScale = new Vector3(.5f, .5f, .5f) * Mathf.Clamp(
                    Mathf.Abs(Mathf.Sin(Time.time * 5f)), .7f, 1f
                );
        }
        public void SetMoneyBalanceTextDisplay(int amount)
        {
            _moneyBalanceText.text = $"Wallet ${amount}";
        }
        public void SetMoneyReceivedTextDisplay(int amount)
        {
            _moneyWonText.text = $"Money Won Latest ${amount}";
        }
        public void SetMoneyMultiplierTextDisplay(int multiplier)
        {
            _multiplierText.text = $"x{multiplier}";
        }

    }
}