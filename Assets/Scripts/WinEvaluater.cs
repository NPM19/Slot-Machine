using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlotMachine
{
    public static class WinEvaluater
    {
        public static int MultiplierUsed { get; private set; }

        public static int CalculateWinAmount(int cashIn, Symbol[] spinResult)
        {
            var spinResultList = spinResult.ToList();
            MultiplierUsed = 0;
            if (CheckForAllIdenticalItem(spinResultList))
            {
                MultiplierUsed = spinResult[0].PayoutMultiplier;
                int winAmount = cashIn * MultiplierUsed;
                return winAmount;
            }

            /*
            BONUS WIN CONDITION: when 2 same symbol is in a resultant spin.
            */
            
            // if (CheckForPartialIdenticalItem<Symbol>(spinResultList, .5f, symbol => symbol.ID, symbol => 1, out Symbol outSymbol))
            // {
            //     if (outSymbol)
            //     {
            //         MultiplierUsed = outSymbol.PayoutMultiplier / 2;
            //     }
            //     else
            //     {
            //         MultiplierUsed = 0;
            //     }
            //     int winAmount = Mathf.CeilToInt((cashIn / 10f) * MultiplierUsed);
            //     return winAmount;
            // }

            return 0;
        }
        // private static bool CheckForPartialIdenticalItem<T>(List<T> list, float percentageMatch, Func<T, int> getKey, Func<T, int> getValue, out T outSymbol)
        // {
        //     if (list.Count <= 0)
        //     {
        //         Debug.LogWarning("list is empty.");
        //         outSymbol = default;
        //         return false;
        //     }

        //     if (list.Count == 1)
        //     {
        //         outSymbol = list[0];
        //         return true;
        //     }

        //     Dictionary<int, int> iDsByFrequency = new Dictionary<int, int>(3);
        //     Dictionary<int, T> filteredIDsByFirstUniqueObject = new();

        //     for (int i = 0; i < list.Count; ++i)
        //     {
        //         T item = list[i];
        //         int key = getKey(item);
        //         int value = getValue(item);

        //         if (iDsByFrequency.TryAdd(key, value))
        //         {
        //             filteredIDsByFirstUniqueObject.Add(key, item);
        //         }
        //         else
        //         {
        //             iDsByFrequency[key] += value;
        //         }
        //     }

        //     List<T> validItems = new();
        //     foreach (var keyValuePair in iDsByFrequency)
        //     {
        //         if (((float)keyValuePair.Value) / list.Count > percentageMatch)
        //         {
        //             validItems.Add(filteredIDsByFirstUniqueObject[keyValuePair.Key]);
        //         }
        //     }

        //     if (validItems.Count > 0)
        //     {
        //         outSymbol = validItems[UnityEngine.Random.Range(0, validItems.Count)];
        //     }
        //     else
        //     {
        //         outSymbol = default;
        //     }

        //     return true;
        // }
        private static bool CheckForAllIdenticalItem<T>(List<T> list)
        {
            if (list.Count <= 0)
            {
                Debug.LogWarning("list is empty.");
                return false;
            }

            if (list.Count == 1)
                return true;

            T first = list[0];

            for (int i = 1; i < list.Count; ++i)
            {
                // This comparision is done using the custom implementation in Symbol class as it implements IEqualityComparer<T>
                if (!EqualityComparer<T>.Default.Equals(first, list[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}