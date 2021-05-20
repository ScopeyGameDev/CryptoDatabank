using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnumBuySell { Buy, Sell };
[System.Serializable]
public class BuySellCrypto
{
    public EnumBuySell BuySell;
    public float Price;
    public float Value;

    public BuySellCrypto(EnumBuySell _BuySell, float _Price, float _Value)
	{
        BuySell = _BuySell;
        Price = _Price;
        Value = _Value;
	}
}

[System.Serializable]
public class Cryptocurrency
{
    public string Name;
    public float Balance;
    public float Price;
    public float Profit
	{
        get { return CalculateProfits(); }
	}

    public List<BuySellCrypto> BuySellCryptos = new List<BuySellCrypto>();

    void BuySell(EnumBuySell BuySell, float _Price, float _Value)
	{
        BuySellCrypto BuySellCrypto = new BuySellCrypto(BuySell, _Price, _Value);
        BuySellCryptos.Add(BuySellCrypto);
        Balance = _Price;
	}

    public void ChangePrice(float _Price)
	{
        Price = _Price;
	}

    float CalculateProfits()
	{
        float Spent = 0;

		foreach (BuySellCrypto item in BuySellCryptos)
		{
            if (item.BuySell == EnumBuySell.Buy)
			{
                Spent += item.Value;
			}
			else
			{
                Spent -= item.Value;
			}
		}

        float CurrentValue = Balance * Price;

        return CurrentValue - Spent;
	}

    void CreateCryptocurrency(string _Name, float _Balance, float _Price)
	{
        Name = _Name;
        Balance = _Balance;
        Price = _Price;
	}
}
