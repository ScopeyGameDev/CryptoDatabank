using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CryptocurrencyManager : MonoBehaviour
{
    [SerializeField] List<Cryptocurrency> Cryptocurrencies = new List<Cryptocurrency>();

    [SerializeField] TMP_Text TitleText;

    [SerializeField] TMP_Text BalanceText;
    [SerializeField] TMP_Text PriceText;
    [SerializeField] TMP_Text ProfitText;

    [SerializeField] TMP_InputField BalanceInputField;
    [SerializeField] TMP_InputField PriceInputField;

    [SerializeField] Button ChangeButton;
    [SerializeField] Button BuyButton;
    [SerializeField] Button SellButton;

    void Start()
    {
        if (Cryptocurrencies.Count == 0)
            return;

        UpdateUI();

        ChangeButton.onClick.AddListener(Change);
    }

    void UpdateUI()
	{
        TitleText.text = Cryptocurrencies[0].Name;
        BalanceText.text = Cryptocurrencies[0].Balance.ToString();
        PriceText.text = Cryptocurrencies[0].Price.ToString("$0.00");
        ProfitText.text = Cryptocurrencies[0].Profit.ToString("$0.00");
    }

    void Change()
	{
        float BalanceInput = float.Parse(BalanceInputField.text);
        float PriceInput = float.Parse(PriceInputField.text);
        Cryptocurrencies[0].Balance = BalanceInput;
        Cryptocurrencies[0].Price = PriceInput;
        UpdateUI();
	}
}
