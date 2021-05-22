using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CryptoDatabank
{
    public class CryptocurrencyManager : MonoBehaviour
    {
		#region Variables
		[SerializeField] List<Cryptocurrency> Cryptocurrencies = new List<Cryptocurrency>();
        int SelectedCryptocurrency;

        [Header("Title")]
        [SerializeField] TMP_Text TitleText;

        [Header("Information")]
        [SerializeField] TMP_Text BalanceText;
        [SerializeField] TMP_Text PriceText;
        [SerializeField] TMP_Text SpentText;
        [SerializeField] TMP_Text ProfitText;

        [Header("Input Fields")]
        [SerializeField] TMP_InputField BalanceInputField;
        [SerializeField] TMP_InputField PriceInputField;

        [Header("Buttons")]
        [SerializeField] Button RefreshButton;
        [SerializeField] Button ChangeButton;
        [SerializeField] Button BuyButton;
        [SerializeField] Button SellButton;

        [Header("List")]
        [SerializeField] TMP_Text AudPriceText;
        [SerializeField] TMP_Text TotalAudPriceText;
        [SerializeField] Transform ListContent;
        List<CryptoSelect> CreatedCryptoSelects = new List<CryptoSelect>();
        [SerializeField] CryptoSelect CryptoSelectPrefab;
        #endregion

        void Start()
        {
            FindCryptocurrencies();

            SelectedCryptocurrency = 1;
            UpdateUI();

            RefreshButton.onClick.AddListener(FindCryptocurrencies);
            ChangeButton.onClick.AddListener(Change);

            Task<string> TransactionTask = Task<string>.Run(() => CoinspotAPI.CoinspotAPI.ListTransactionHistory("cc58e215110632a0805dc7d2bee1f4dc", "EGVWJM3ATTHPE18FX5LQF5EVA0BF4QVB47FLK3Y71QK7F4WLL0XTY72A8TUWP52CXYUQJ695GJFMNBVV"));
            Debug.Log(TransactionTask.Result);
        }

        void FindCryptocurrencies()
		{
            Cryptocurrencies.Clear();
			foreach (CryptoSelect CryptoSelect in CreatedCryptoSelects)
			{
                Destroy(CryptoSelect.gameObject);
			}
            CreatedCryptoSelects.Clear();

            Task<string> BalanceTask = Task<string>.Run(() => CoinspotAPI.CoinspotAPI.ListMyBalances("cc58e215110632a0805dc7d2bee1f4dc", "EGVWJM3ATTHPE18FX5LQF5EVA0BF4QVB47FLK3Y71QK7F4WLL0XTY72A8TUWP52CXYUQJ695GJFMNBVV"));
            string Balances = BalanceTask.Result;
            int Start = Balances.IndexOf("\"balances\":[{");
            Start += "\"balances\":[{".Length;

            while (Balances.IndexOf("\"", Start) != -1)
            {
                Start = Balances.IndexOf("\"", Start);
                int End = Balances.IndexOf("\"", Start + 1);
                string Name = (Balances.Substring(Start + 1, End - (Start + 1)));

                Start = Balances.IndexOf("\"balance\":", End);
                Start += "\"balance\":".Length;
                End = Balances.IndexOf(",", Start);
                double Balance = double.Parse(Balances.Substring(Start, End - Start));

                Start = Balances.IndexOf("\"audbalance\":", End);
                Start += "\"audbalance\":".Length;
                End = Balances.IndexOf(",", Start);
                double AudBalance = double.Parse(Balances.Substring(Start, End - Start));

                Start = Balances.IndexOf("\"rate\":", End);
                Start += "\"rate\":".Length;
                End = Balances.IndexOf("}", Start);
                double Rate = double.Parse(Balances.Substring(Start, End - Start));

                AddCryptocurrency(new Cryptocurrency(Name, Balance, Rate));
            }

            CryptocurrencySelected(SelectedCryptocurrency);

            CreatedCryptoSelects[0].gameObject.SetActive(false);
        }

		void UpdateUI()
        {
            AudPriceText.text = "AUD: " + Cryptocurrencies[0].Balance.ToString("$0.00");

            double TotalBalance = 0;
            foreach (Cryptocurrency Crypto in Cryptocurrencies)
            {
                TotalBalance += Crypto.Profit;
            }
            TotalAudPriceText.text = "~ Total AUD: " + TotalBalance.ToString("$0.00");

            TitleText.text = Cryptocurrencies[SelectedCryptocurrency].Name;
            BalanceText.text = Cryptocurrencies[SelectedCryptocurrency].Balance.ToString();
            PriceText.text = "$" + Cryptocurrencies[SelectedCryptocurrency].Price.ToString();
            SpentText.text = Cryptocurrencies[SelectedCryptocurrency].Spent.ToString("$0.00");
            ProfitText.text = Cryptocurrencies[SelectedCryptocurrency].Profit.ToString("$0.00");
        }

        void Change()
        {
            double BalanceInput = double.Parse(BalanceInputField.text);
            double PriceInput = double.Parse(PriceInputField.text);
            Cryptocurrencies[0].Balance = BalanceInput;
            Cryptocurrencies[0].Price = PriceInput;
            UpdateUI();
        }

        internal void CryptocurrencySelected(int _SelectedCryptocurrency)
		{
            SelectedCryptocurrency = _SelectedCryptocurrency;
            UpdateUI();
		}

        internal void AddCryptocurrency(Cryptocurrency CryptoToAdd)
		{
            Cryptocurrencies.Add(CryptoToAdd);
            AddCryptoToList(CryptoToAdd);
        }

        void AddCryptoToList(Cryptocurrency CryptoToAdd)
		{
            CryptoSelect CreatedCryptoSelect = Instantiate(CryptoSelectPrefab, ListContent);
            CreatedCryptoSelects.Add(CreatedCryptoSelect);
            CreatedCryptoSelect.Assign(this, CryptoToAdd, Cryptocurrencies.Count - 1);
            
        }
    }
}