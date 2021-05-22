using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoDatabank
{
    internal enum EnumBuySell { Buy, Sell };
    [System.Serializable]
    internal class BuySellCrypto
    {
        [SerializeField] internal EnumBuySell BuySell;
        [SerializeField] internal double Price;
        internal double Balance;
        [SerializeField] internal double Value;

        internal BuySellCrypto(EnumBuySell _BuySell, double _Balance, double _Price, double _Value)
        {
            BuySell = _BuySell;
            Price = _Price;
            Balance = _Balance;
            Value = _Value;
        }
    }

    [System.Serializable]
    internal class Cryptocurrency
    {
        [SerializeField] internal string Name;
        [SerializeField] internal double Balance;
        [SerializeField] internal double Price;
        internal double Spent
		{
            get { return CalculateSpent(); }
		}
        internal double Profit
        {
            get { return CalculateProfits(); }
        }

        [SerializeField] internal List<BuySellCrypto> BuySellCryptos = new List<BuySellCrypto>();

        internal Cryptocurrency(string _Name, double _Balance, double _Price)
        {
            Name = _Name;
            Balance = _Balance;
            Price = _Price;
        }

        void BuySell(EnumBuySell BuySell, double _Price, double _Value)
        {
            BuySellCrypto BuySellCrypto = new BuySellCrypto(BuySell, _Price, 0, _Value);
            BuySellCryptos.Add(BuySellCrypto);
            Balance = _Price;
        }

        internal void ChangePrice(float _Price)
        {
            Price = _Price;
        }

        double CalculateProfits()
        {
            double CurrentValue = Balance * Price;
            return CurrentValue - Spent;
        }

        double CalculateSpent()
		{
            double _Spent = 0;
            foreach (BuySellCrypto item in BuySellCryptos)
            {
                if (item.BuySell == EnumBuySell.Buy)
                {
                    _Spent += item.Value;
                }
                else
                {
                    _Spent -= item.Value;
                }
            }
            return _Spent;
        }
    }
}