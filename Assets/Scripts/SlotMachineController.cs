using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SlotMachine
{
    public enum SlotMachineState
    {
        READY, SPINNING, RESULT, WON, LOST
    }
    public class SlotMachineController : MonoBehaviour
    {
        [SerializeField] private GameObject _slotHandleIdle;
        [SerializeField] private GameObject _slotHandleUsed;
        [SerializeField] private ReelManager _reelManager;
        [SerializeField] private List<Symbol> _symbolsInPlay;
        [SerializeField, Range(0f, 3f)] private float _showResultTime;
        [SerializeField, Range(0f, 3f)] private float _showReelResultTimeInterval;
        [SerializeField] private int _cashInAmount;

        public Symbol[] SpinResult { get; private set; }
        public SlotMachineState SlotMachineState { get; private set; }

        private void Start()
        {
            _reelManager.InitializeReels(_symbolsInPlay);
            SlotMachineState = SlotMachineState.READY;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 worldPosition = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Handle"))
                    {
                        SlotMachineState = SlotMachineState.SPINNING;
                        DisableHandleUse();
                        UIManager.Instance.SetMoneyBalanceTextDisplay(_cashInAmount);
                        SpinResult = GenerateResult();
                        _reelManager.StartReeling(_showResultTime, _showReelResultTimeInterval, OnSpinResultTimeReached);
                        // Debug.Log($"Slot Result : [{SpinResult[0].ID}, {SpinResult[1].ID}, {SpinResult[2].ID}]");
                    }
                }
            }
        }

        private void OnSpinResultTimeReached()
        {
            SlotMachineState = SlotMachineState.RESULT;
            _reelManager.FinishReeling(OnSpinFinished);
        }
        private void OnSpinFinished()
        {
            int moneyAmount = WinEvaluater.CalculateWinAmount(_cashInAmount, SpinResult);
            if (WinEvaluater.CalculateWinAmount(_cashInAmount, SpinResult) > 0)
            {
                SlotMachineState = SlotMachineState.WON;
            }
            else
            {
                SlotMachineState = SlotMachineState.LOST;
            }
            UIManager.Instance.SetMoneyReceivedTextDisplay(moneyAmount);
            UIManager.Instance.SetMoneyMultiplierTextDisplay(WinEvaluater.MultiplierUsed);
            // Debug.Log($"You {SlotMachineState}, money received ${moneyAmount}.");
            EnableHandleUse();
            SlotMachineState = SlotMachineState.READY;
        }
        private Symbol[] GenerateResult()
        {
            return new Symbol[]{
                WeightedRandomPicker.GetRandomSymbol(_symbolsInPlay, x => x.Weight),
                WeightedRandomPicker.GetRandomSymbol(_symbolsInPlay, x => x.Weight),
                WeightedRandomPicker.GetRandomSymbol(_symbolsInPlay, x => x.Weight)
            };
        }

        private void DisableHandleUse()
        {
            _slotHandleIdle.SetActive(false);
            _slotHandleUsed.SetActive(true);
        }
        private void EnableHandleUse()
        {
            _slotHandleIdle.SetActive(true);
            _slotHandleUsed.SetActive(false);
        }
    }
}