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

        [Header("Buttons")]
        [SerializeField] Button RefreshButton;

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

            Task<string> TransactionTask = Task<string>.Run(() => CoinspotAPI.CoinspotAPI.ListTransactionHistory("cc58e215110632a0805dc7d2bee1f4dc", "EGVWJM3ATTHPE18FX5LQF5EVA0BF4QVB47FLK3Y71QK7F4WLL0XTY72A8TUWP52CXYUQJ695GJFMNBVV", "BTC"));
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

            //for (int i = 1; i < Cryptocurrencies.Count; i++)
            //{
            //    FindTransactionHistory(i);
            //}
        }

        void FindTransactionHistory(int Crypto)
		{
            Task<string> TransactionTask = Task<string>.Run(() => CoinspotAPI.CoinspotAPI.ListTransactionHistory("cc58e215110632a0805dc7d2bee1f4dc", "EGVWJM3ATTHPE18FX5LQF5EVA0BF4QVB47FLK3Y71QK7F4WLL0XTY72A8TUWP52CXYUQJ695GJFMNBVV", Cryptocurrencies[Crypto].Name));
            string Transactions = TransactionTask.Result;
            Debug.Log(Transactions);

            int BuyOrdersIndex = Transactions.IndexOf("\"buyorders\":") + "\"buyorders\":".Length;
            int SellOrdersIndex = Transactions.IndexOf(",\"sellorders\":");

            string BuyOrdersString = Transactions.Substring(BuyOrdersIndex, SellOrdersIndex - BuyOrdersIndex);
            Debug.Log(BuyOrdersString);

			int Start = 0;
			int End = 0;
			while (BuyOrdersString.IndexOf("{", Start) != -1)
			{
				Start = BuyOrdersString.IndexOf("\"amount\":", Start);
				Start += "\"amount\":".Length;
				End = BuyOrdersString.IndexOf(",", Start);
				double Amount = double.Parse(BuyOrdersString.Substring(Start, End - Start));

				Start = BuyOrdersString.IndexOf("\"audtotal\":", Start);
				Start += "\"audtotal\":".Length;
				End = BuyOrdersString.IndexOf("}", Start);
				double AudTotal = double.Parse(BuyOrdersString.Substring(Start, End - Start));

				Debug.Log(/*"Name: " + Name + */" Amount: " + Amount + " AUDTotal: " + AudTotal);
                Cryptocurrencies[Crypto].BuySell(EnumBuySell.Buy, Amount, (AudTotal / Amount) * 0.990099f, AudTotal);
			}

            SellOrdersIndex = Transactions.IndexOf(",\"sellorders\":") + ",\"sellorders\":".Length;
            string SellOrdersString = Transactions.Substring(SellOrdersIndex);

            Start = 0;
            End = 0;
            while (SellOrdersString.IndexOf("{", Start) != -1)
            {
                Start = SellOrdersString.IndexOf("\"amount\":", Start);
                Start += "\"amount\":".Length;
                End = SellOrdersString.IndexOf(",", Start);
                double Amount = double.Parse(SellOrdersString.Substring(Start, End - Start));

                Start = SellOrdersString.IndexOf("\"audtotal\":", Start);
                Start += "\"audtotal\":".Length;
                End = SellOrdersString.IndexOf("}", Start);
                double AudTotal = double.Parse(SellOrdersString.Substring(Start, End - Start));

                Debug.Log(/*"Name: " + Name + */" Amount: " + Amount + " AUDTotal: " + AudTotal);
                Cryptocurrencies[Crypto].BuySell(EnumBuySell.Sell, Amount, (AudTotal / Amount) * 1.009711f, AudTotal);
            }
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