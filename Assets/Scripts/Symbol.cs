using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class Symbol : MonoBehaviour, IEqualityComparer<Symbol>
    {
        [SerializeField] private int id;
        [SerializeField] private int weight;
        [SerializeField] private int payoutMultiplier;

        public int ID => id;
        public int Weight => weight;
        public int PayoutMultiplier => payoutMultiplier;

        public bool Equals(Symbol x, Symbol y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Symbol obj)
        {
            return HashCode.Combine(obj.ID);
        }
    }
}