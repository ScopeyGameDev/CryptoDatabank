using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoinspotAPI
{
    /// <summary>
    /// https://github.com/sdoddler/CoinspotAPI-CSharp
    /// </summary>
    static class CoinspotAPI
    {


        /// <summary>
        /// Returns Open orders for coin -- 
        ///Response
        ///status - ok, error
        ///buyorders - array containing all the open buy orders
        ///sellorders - array containing all the open sell orders
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> ListOpenOrders(string apiKey, string apiSecret, string coin, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/orders", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        ///<summary>
        ///Returns completed last 1000 completed orders for coin --
        ///Response
        ///status - ok, error
        ///orders - list of the last 1000 completed orders
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> ListOrderHistory(string apiKey, string apiSecret, string coin, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/orders/history", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        ///<summary>
        ///Returns Deposit address for Coin -- 
        ///Response
        ///status - ok, error
        ///address - your deposit address for the coin
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> MyDepositAddress(string apiKey, string apiSecret, string coin, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/my/coin/deposit", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        ///<summary>
        ///WARNING: THIS DOES NOT APPEAR TO WORK AS INTENDED -- Returns estimated timeframe to fill quickbuy order --
        ///Response
        ///status - ok, error
        ///quote - the rate per coin
        ///timeframe - estimate hours to wait for trade to complete (0 = immediate trade)
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="amount">amount of coins</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> QuickBuyQuote(string apiKey, string apiSecret, string coin, float amount, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\",\"amount\":\"" + amount + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/quote/buy", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        ///<summary>
        ///WARNING: THIS DOES NOT APPEAR TO WORK AS INTENDED -- Returns estimated timeframe to fill quicksell order --
        ///Response
        ///status - ok, error
        ///quote - the rate per coin
        ///timeframe - estimate hours to wait for trade to complete (0 = immediate trade)
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="amount">amount of coins</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> QuickSellQuote(string apiKey, string apiSecret, string coin, float amount, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\",\"amount\":\"" + amount + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/quote/sell", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        /// <summary>
        ///Returns list of all Wallet Balances, max 100 results
        ///Response
        ///status - ok, error
        ///balances - object containing one property for each coin with your balance for that coin.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>       
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> ListMyBalances(string apiKey, string apiSecret, bool debug = false)
        {
            string postData = "";
            string response = await APIQuery(apiKey, apiSecret, "/my/balances", postData, "https://www.coinspot.com.au/api/v2/ro", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        ///<summary>
        ///List a Coins Balance -- Doesn't work.
        ///Response
        ///status - ok, error
        ///balance - object containing one property with your balance, AUD value and rate for that coin
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="coin">"BTC", "DOGE", "ETH" etc.</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        /// 

        // public static async Task<string> ListCoinBalance(string apiKey, string apiSecret, string coin, bool debug = false)
        // {
        //     string postData = $"\"cointype\":\"" + coin + "\"";

        //     string response = await APIQuery(apiKey, apiSecret, "/ro/my/balances/"+coin, postData, "https://www.coinspot.com.au/api", debug);
        //     if (debug) Debug.WriteLine(response);
        //     return response;
        //  }



        /// <summary>
        ///Returns list of 100(max) of your open orders -- 
        ///Response
        ///status - ok, error
        ///balances - object containing one property for each coin with your balance for that coin.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>       
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> ListMyOrders(string apiKey, string apiSecret, bool debug = false)
        {
            string postData = "";
            string response = await APIQuery(apiKey, apiSecret, "/my/orders", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        //List Deposit History
        //Url
        ///api/ro/my/deposits
        //Inputs
        //startdate - (optional) format 'YYYY-MM-DD'
        //enddate - (optional)format 'YYYY-MM-DD'
        //Response
        //status - ok, error
        //deposits - array containing your AUD deposit history
        public static async Task<string> ListDepositHistory(string apiKey, string apiSecret, string startDate = "", string endDate = "", bool debug = false)
        {
            string postData = "";
            if (startDate != "")
            {
                postData += $"\"startdate\":\"{startDate}\"";
            }
            if (endDate != "")
            {
                if (postData != "") postData += ",";
                postData += $"\"enddate\":\"{endDate}\"";
            }
            string response = await APIQuery(apiKey, apiSecret, "/ro/my/deposits", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }



        //List Withdrawal History
        //Inputs
        //startdate - (optional) format 'YYYY-MM-DD'
        //enddate - (optional)format 'YYYY-MM-DD'
        //Response
        //status - ok, error
        //withdrawals - array containing your AUD withdrawal history
        public static async Task<string> ListWithdrawalHistory(string apiKey, string apiSecret, string startDate = "", string endDate = "", bool debug = false)
        {
            string postData = "";
            if (startDate != "")
            {
                postData += $"\"startdate\":\"{startDate}\"";
            }
            if (endDate != "")
            {
                if (postData != "") postData += ",";
                postData += $"\"enddate\":\"{endDate}\"";
            }
            string response = await APIQuery(apiKey, apiSecret, "/ro/my/withdrawals", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        //List My Transaction History
        //Inputs
        //startdate - (optional) format 'YYYY-MM-DD'
        //enddate - (optional)format 'YYYY-MM-DD'
        //Response
        //status - ok, error
        //buyorders - array containing your buy order history
        //sellorders - array containing your sell order history
        public static async Task<string> ListTransactionHistory(string apiKey, string apiSecret, string coin = "", string startDate = "", string endDate = "", bool debug = false)
        {
            string postData = "";

            string queryUrl = "/ro/my/transactions";
            if (coin != "")
            {
                queryUrl += "/" + coin;
                postData += $"\"cointype\":\"" + coin + "\"";
            }
            if (startDate != "")
            {
                if (postData != "") postData += ",";
                postData += $"\"startdate\":\"{startDate}\"";
            }
            if (endDate != "")
            {
                if (postData != "") postData += ",";
                postData += $"\"enddate\":\"{endDate}\"";
            }
            string response = await APIQuery(apiKey, apiSecret, queryUrl, postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        //List My Open Transactions
        //Url
        ///api/ro/my/transactions/open
        //Inputs
        //none
        //Response
        //status - ok, error
        //buyorders - array containing your open buy orders
        //sellorders - array containing your open sell orders

        //List My Coins Open Transactions
        //Url
        ///api/ro/my/transactions/:cointype / open
        //Inputs
        //cointype - the coin shortname, example value 'BTC', 'LTC', 'DOGE'
        //Response
        //status - ok, error
        //buyorders - array containing your coins open buy orders
        //sellorders - array containing your coins open sell orders
        public static async Task<string> ListMyOpenOrders(string apiKey, string apiSecret, string coin = "", bool debug = false)
        {
            string postData = "";

            string queryUrl = "/ro/my/transactions";
            if (coin != "")
            {
                queryUrl += "/" + coin;
                postData += $"\"cointype\":\"" + coin + "\"";
            }

            queryUrl += "/open";

            string response = await APIQuery(apiKey, apiSecret, queryUrl, postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        //List My Send & Receive Transaction History
        //Url
        ///api/ro/my/sendreceive
        //Inputs
        //none
        //Response
        //status - ok, error
        //sendtransactions - array containing your coin send transaction history
        //receivetransactions - array containing your coin receive transaction history

        public static async Task<string> SendReceiveTransactions(string apiKey, string apiSecret, bool debug = false)
        {
            string postData = "";

            string queryUrl = "/ro/my/sendreceive";

            string response = await APIQuery(apiKey, apiSecret, queryUrl, postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        //List My Affiliate Payments
        //Url
        //
        //Inputs
        //none
        //Response
        //status - ok, error
        //payments - array containing one object for each completed affiliate payment


        public static async Task<string> ListMyAffiliatePayments(string apiKey, string apiSecret, bool debug = false)
        {
            string postData = "";

            string queryUrl = "/ro/my/affiliatepayments";

            string response = await APIQuery(apiKey, apiSecret, queryUrl, postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        //List My Referral Payments
        //Url
        ///api/ro/my/referralpayments
        //Inputs
        //none
        //Response
        //status - ok, error
        //payments - array containing one object for each completed referral payment
        public static async Task<string> ListMyReferralPayments(string apiKey, string apiSecret, bool debug = false)
        {
            string postData = "";

            string queryUrl = "/ro/my/referralpayments";

            string response = await APIQuery(apiKey, apiSecret, queryUrl, postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }



        /// <summary>
        ///Places buy order on the market -- 
        ///Response
        ///status - ok, error
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>       
        /// <param name="debug"></param>
        /// <param name="coin">"BTC","DOGE","ETH" etc.</param>
        /// <param name="amount">Amount of coins you wish to purchase</param>
        /// <param name="rate">rate you're willing to pay per coin..</param>
        /// <returns></returns>
        public static async Task<string> PlaceBuyOrder(string apiKey, string apiSecret, string coin, double amount, double rate, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\",\"amount\":\"" + amount + "\",\"rate\":\"" + rate + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/my/buy", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        /// <summary>
        ///Places buy order on the market -- 
        ///Response
        ///status - ok, error
        ///id - order id
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>       
        /// <param name="debug"></param>
        /// <param name="coin">"BTC","DOGE","ETH" etc.</param>
        /// <param name="amount">Amount of coins you wish to purchase</param>
        /// <param name="rate">rate you're willing to pay per coin..</param>
        /// <returns></returns>
        public static async Task<string> PlaceSellOrder(string apiKey, string apiSecret, string coin, double amount, double rate, bool debug = false)
        {
            string postData = $"\"cointype\":\"" + coin + "\",\"amount\":\"" + amount + "\",\"rate\":\"" + rate + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/my/sell", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        ///<summary>
        ///Cancel Buy Order
        ///Response
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="id">id reference of the order</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> CancelBuyOrder(string apiKey, string apiSecret, string id, bool debug = false)
        {
            string postData = $"\"id\":\"" + id + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/my/buy/cancel", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }

        ///<summary>
        ///Cancel Sell Order
        ///Response
        ///</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="id">id reference of the order</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static async Task<string> CancelSellOrder(string apiKey, string apiSecret, string id, bool debug = false)
        {
            string postData = $"\"id\":\"" + id + "\"";
            string response = await APIQuery(apiKey, apiSecret, "/my/sell/cancel", postData, "https://www.coinspot.com.au/api", debug);
            if (debug) Debug.WriteLine(response);
            return response;
        }


        public static async Task<string> APIQuery(string apiKey, string apiSecret, string queryPath, string postData = "", string endpointUrl = "https://www.coinspot.com.au/api", bool debug = false)
        {
            HttpResponseMessage json;

            long nonce = DateTime.Now.Ticks;
            var requestUri = new Uri(endpointUrl + queryPath); //("https://www.coinspot.com.au/api/ro/my/balances"); 

            if (postData != "") postData = "," + postData;
            string nonceString = "{\"nonce\":\"" + nonce + "\"" + postData + "}";

            if (debug) Debug.WriteLine(nonceString);


            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            requestMessage.Content = new StringContent(nonceString, Encoding.UTF8, "application/json");

            var signature = HexHash(nonceString, apiSecret);
            if (debug) Debug.WriteLine(signature);

            requestMessage.Content.Headers.Add("key", apiKey);
            requestMessage.Content.Headers.Add("sign", signature);

            if (debug) Debug.WriteLine(requestMessage.ToString());
            if (debug) Debug.WriteLine(requestMessage.Content.ToString());

            HttpClient httpClient = new HttpClient
            {

                BaseAddress = requestUri
            };
            var response = await httpClient.PostAsync(requestUri, requestMessage.Content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (debug) Debug.WriteLine(responseString);
            return responseString;
        }


        private static bool CheckDecimals(decimal valueDecimal, int decimalPlaces)
        {
            if (Decimal.Round(valueDecimal, decimalPlaces) != valueDecimal)
            {
                return false;
            }
            return true;
        }

        static string HexHash(string message, string key)
        {
            byte[] keyByte = new ASCIIEncoding().GetBytes(key);
            byte[] messageBytes = new ASCIIEncoding().GetBytes(message);

            byte[] hashmessage = new HMACSHA512(keyByte).ComputeHash(messageBytes);

            // to lowercase hexits
            return String.Concat(Array.ConvertAll(hashmessage, x => x.ToString("x2")));
        }
    }
}