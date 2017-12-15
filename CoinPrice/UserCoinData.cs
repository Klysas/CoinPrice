using System.Globalization;
using System.Threading.Tasks;

namespace CoinPrice
{
	public class UserCoinData : IUserCoinData
	{
		//========================================================
		//	Variables
		//========================================================
		//--------------------------------------------------------
		//	Private
		//--------------------------------------------------------

		private ICoinDataAccess coinAccess;

		//========================================================
		//	Constructors
		//========================================================

		public UserCoinData(ICoinDataAccess coinAccess)
		{
			coinAccess.ThrowIfArgumentIsNull("coinAccess");

			this.coinAccess = coinAccess;
		}

		//========================================================
		//	Properties
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public float BoughtPriceInEur { get; set; }
		public float BoughtPriceInUSD { get; set; }
		public string CoinName { get; set; }
		public string CoinUrlName { get; set; }
		public double CurrentAmount { get; set; }
		public float CurrentPriceInEur { get; set; }
		public float CurrentPriceInUSD { get; set; }

		//========================================================
		//	Methods
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public async Task UpdateCurrentPrice()
		{
			var result = await coinAccess.GetCoinDataAsync(CoinUrlName);

			CurrentPriceInEur = float.Parse(result.price_eur, CultureInfo.InvariantCulture);
			CurrentPriceInUSD = float.Parse(result.price_usd, CultureInfo.InvariantCulture);
		}
	}
}
