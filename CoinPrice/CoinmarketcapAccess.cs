using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace CoinPrice
{
	/// <summary>
	/// This class used to access and retrieve data from https://coinmarketcap.com site.
	/// </summary>
	public class CoinmarketcapAccess : ICoinDataAccess
	{
		//========================================================
		//	Variables
		//========================================================

		const string CoinmarketcapApiSite = "https://api.coinmarketcap.com/v1/ticker/";

		//--------------------------------------------------------
		//	Private
		//--------------------------------------------------------

		private RestClient client;

		//========================================================
		//	Constructors
		//========================================================

		public CoinmarketcapAccess()
		: this(CoinmarketcapApiSite)
		{
			// Empty.
		}

		public CoinmarketcapAccess(string url)
		{
			client = new RestClient();
			client.BaseUrl = new Uri(url);
		}

		//========================================================
		//	Methods
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public async Task<CoinJsonItem> GetCoinDataAsync(string coinName)
		{
			if (string.IsNullOrEmpty(coinName))
				throw new ArgumentException("coinName parameter is null or empty.");

			var request = new RestRequest();
			request.RequestFormat = RestSharp.DataFormat.Json;
			request.Resource = coinName.Trim() + "/?convert=EUR";

			var response = await client.ExecuteTaskAsync(request);

			if (response.ErrorException != null)
			{
				throw response.ErrorException;
			}

			CoinJsonItem item;
			if (response.Content.Contains("error"))
			{
				item = JsonConvert.DeserializeObject<CoinJsonItem>(response.Content);
			}
			else
			{
				item = JsonConvert.DeserializeObject<List<CoinJsonItem>>(response.Content)[0];
			}

			return item;
		}

		public async Task<UserCoinData> UpdateCoinDataAsync(UserCoinData userCoin)
		{
			var result = await GetCoinDataAsync(userCoin.CoinUrlName);

			userCoin.CurrentPriceInEur = float.Parse(result.price_eur, CultureInfo.InvariantCulture);
			userCoin.CurrentPriceInUSD = float.Parse(result.price_usd, CultureInfo.InvariantCulture);

			return userCoin;
		}
	}
}
