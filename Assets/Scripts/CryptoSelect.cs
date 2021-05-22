using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CryptoDatabank
{
    internal class CryptoSelect : MonoBehaviour
    {
        CryptocurrencyManager CryptocurrencyManager;
        Cryptocurrency Cryptocurrency;
        [SerializeField] TMP_Text Title;

        internal void Assign(CryptocurrencyManager _CryptocurrencyManager, Cryptocurrency _Cryptocurrency, int Order)
		{
            CryptocurrencyManager = _CryptocurrencyManager;
            Cryptocurrency = _Cryptocurrency;
            Title.text = Cryptocurrency.Name;
            GetComponent<Button>().onClick.AddListener(delegate { CryptocurrencyManager.CryptocurrencySelected(Order); });
		}
    }
}