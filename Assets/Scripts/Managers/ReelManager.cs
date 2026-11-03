using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class ReelManager : MonoBehaviour
    {
        [SerializeField, Range(1f, 100f)] private float _speed;
        [SerializeField, Range(1, 10)] private int _symbolsPerReel;
        [SerializeField, Range(0f, 10f)] private float _spacingBetweenSymbols;
        [SerializeField] private Transform[] _reelAnchorCenters;
        [SerializeField] private SlotMachineController _slotMachineController;
        [SerializeField] private float _intervalTime;

        private List<List<Symbol>> _reelList;
        private List<bool> _reelRunningStatusList;
        private Symbol[] _spinResult;
        private float _showResultTime;
        private float _reelStopTimeInterval;
        private float _showReelResultTimeInterval;
        private bool _resultTimeReached;
        private bool _allowAnimationAndResultSyncing;
        private int _reelsStoppedIndex;

        private Action _onSpinFinish, _onSpinResultTimeReached;


        private void Awake()
        {
            _spinResult = new Symbol[3];
        }
        public void InitializeReels(List<Symbol> symbols)
        {
            _reelList = new List<List<Symbol>>(3);
            _reelRunningStatusList = new List<bool>(3);

            for (int i = 0; i < 3; ++i)
            {
                _reelList.Add(new List<Symbol>());
                _reelRunningStatusList.Add(true);
            }
            CreateRandomSymbolsForReels(symbols);
        }

        private Coroutine _routineFinish;
        
        public void FinishReeling(Action onSpinFinish)
        {
            if (_routineFinish != null)
            {
                StopCoroutine(_routineFinish);
            }
            _onSpinFinish = onSpinFinish;
            _routineFinish = StartCoroutine(RoutineShowResult());
        }
        System.Collections.IEnumerator RoutineShowResult()
        {
            yield return new WaitForSeconds(_showResultTime);
            _reelsStoppedIndex = -1;
            _allowAnimationAndResultSyncing = true;
        }

        private void StopReelOnResultSymbolReachingPayline()
        {
            if (_reelStopTimeInterval < Time.time)
            {
                _reelStopTimeInterval = Time.time + _intervalTime;
                ++_reelsStoppedIndex;
            }

            if (_reelsStoppedIndex < 0)
                return;

            // Debug.Log(_spinResult[0].ID + "" + _spinResult[1].ID + "" + _spinResult[2].ID);
            float offset = _spinResult[_reelsStoppedIndex].transform.position.y - _reelAnchorCenters[_reelsStoppedIndex].transform.position.y;
            if (offset > 0f)
            {
                _reelRunningStatusList[_reelsStoppedIndex] = false;
                ShiftSymbolsYAxis(_reelsStoppedIndex, offset);

                // Last reel has finished settings the symbol on payline
                if (_reelsStoppedIndex == _reelList.Count - 1)
                {
                    _allowAnimationAndResultSyncing = false;
                    // Run callback function on SlotMachineController
                    _onSpinFinish?.Invoke();
                }
            }
        }

        public void StartReeling(float showResultTime, float showReelResultTimeInterval, Action onSpinResultTimeReached)
        {
            _showResultTime = showResultTime;
            _showReelResultTimeInterval = showReelResultTimeInterval + Time.time;
            _onSpinResultTimeReached = onSpinResultTimeReached;
            _resultTimeReached = false;

            CheckFixResultSymbol();
            for (int i = 0; i < _reelRunningStatusList.Count; ++i)
            {
                _reelRunningStatusList[i] = true;
            }
        }
        private void CreateRandomSymbolsForReels(List<Symbol> symbols)
        {
            int reelIndex = 0;
            foreach (List<Symbol> reelIterator in _reelList)
            {
                Transform parent = new GameObject($"Reel {reelIndex}").transform;
                reelIterator.Capacity = _symbolsPerReel;
                for (int i = 0; i < _symbolsPerReel; ++i)
                {
                    Symbol symbolInstance = Instantiate(symbols[UnityEngine.Random.Range(0, symbols.Count)], _reelAnchorCenters[reelIndex].position + (i * -_spacingBetweenSymbols * Vector3.up), Quaternion.identity);
                    reelIterator.Add(symbolInstance);
                    symbolInstance.transform.parent = parent;
                }
                ++reelIndex;
            }
        }
        private void CheckFixResultSymbol()
        {
            for (int reelIndex = 0; reelIndex < _reelList.Count; ++reelIndex)
            {
                bool resultSymbolFound = false;
                int resultID = _slotMachineController.SpinResult[reelIndex].ID;
                for (int i = 0; i < _symbolsPerReel; ++i)
                {
                    if (_reelList[reelIndex][i].ID == resultID)
                    {
                        _spinResult[reelIndex] = _reelList[reelIndex][i];
                        resultSymbolFound = true;
                        break;
                    }
                }

                if (!resultSymbolFound)
                {
                    int randomResultLocationIndex = UnityEngine.Random.Range(0, _symbolsPerReel);
                    GameObject reelSymbolGettingReplaced = _reelList[reelIndex][randomResultLocationIndex].gameObject;
                    Destroy(reelSymbolGettingReplaced.gameObject);
                    Symbol symbolInstance = Instantiate(_slotMachineController.SpinResult[reelIndex], _reelAnchorCenters[reelIndex].position + (randomResultLocationIndex * -_spacingBetweenSymbols * Vector3.up), Quaternion.identity);
                    _spinResult[reelIndex] = symbolInstance;
                    _reelList[reelIndex][randomResultLocationIndex] = symbolInstance;
                    symbolInstance.transform.parent = reelSymbolGettingReplaced.transform.parent;
                }
            }
        }
        private void ShiftSymbolsYAxis(int reelIndex, float offset)
        {
            Vector3 offsetVector = Vector3.up * offset;
            for (int j = 0; j < _symbolsPerReel; ++j)
            {
                var symbolTransform = _reelList[reelIndex][j].transform;
                symbolTransform.position -= offsetVector;
            }
        }

        private void Update()
        {
            if (_slotMachineController.SlotMachineState == SlotMachineState.SPINNING
                || _slotMachineController.SlotMachineState == SlotMachineState.RESULT)
            {
                if (!_resultTimeReached && _showReelResultTimeInterval < Time.time)
                {
                    _resultTimeReached = true;
                    _onSpinResultTimeReached?.Invoke();
                }
                for (int i = 0; i < _reelList.Count; ++i)
                {
                    // Stop the ith reel from animating.
                    if (!_reelRunningStatusList[i])
                        continue;

                    for (int j = 0; j < _symbolsPerReel; ++j)
                    {
                        var symbolTransform = _reelList[i][j].transform;
                        symbolTransform.position += _speed * Time.deltaTime * Vector3.up;

                        if (symbolTransform.position.y > 1.82f)
                        {
                            int indexOfSymbolToWrapUnder = j - 1;
                            if (indexOfSymbolToWrapUnder < 0)
                            {
                                indexOfSymbolToWrapUnder = _symbolsPerReel - 1;
                            }
                            symbolTransform.position = _reelList[i][indexOfSymbolToWrapUnder].transform.position + (-_spacingBetweenSymbols * Vector3.up);
                        }
                    }
                }

                if (_allowAnimationAndResultSyncing)
                {
                    StopReelOnResultSymbolReachingPayline();
                }
            }
        }
    }
}