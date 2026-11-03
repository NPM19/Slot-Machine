using System;
using System.Collections.Generic;
using System.Linq;

namespace SlotMachine
{
    public static class WeightedRandomPicker
    {
        /// <summary>
        /// This function is used for random symbol selection using weighted probabilities.
        /// </summary>
        /// <param name="symbols">List of items to select random item from.</param>
        /// <param name="weightSelector">Higher value means higher chance to pick at random, and Lower value means lower chance </param>
        /// <returns></returns>
        public static Symbol GetRandomSymbol(List<Symbol> symbols, Func<Symbol, int> weightSelector)
        {
            if (symbols.Count == 0)
            {
                UnityEngine.Debug.LogError("Weighted Random Picker Failed: List is empty for weighted selection");
                return null;
            }

            double totalWeight = symbols.Sum(weightSelector);

            if (totalWeight <= 0)
            {
                UnityEngine.Debug.LogError("Weighted Random Picker Failed: Total weight is less than or equal to 0");
                return null;
            }

            // Setting the range of weights to consider at random it will never be 100% total weight.
            double randomNumber = UnityEngine.Random.value * totalWeight;

            double cumulativeWeight = 0;

            foreach (var symbol in symbols)
            {
                cumulativeWeight += symbol.Weight;

                // when the cumulative weight exceeds randomNumber we get our random symbol.
                if (randomNumber < cumulativeWeight)
                {
                    return symbol;
                }
            }

            // fallback code
            return symbols[UnityEngine.Random.Range(0, symbols.Count)];
        }
    }
}