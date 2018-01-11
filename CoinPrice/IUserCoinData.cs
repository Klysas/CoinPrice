using System.Threading.Tasks;

namespace CoinPrice
{
	public interface IUserCoinData
	{
		/// <summary>
		/// Price for single unit in euros, when coins were bought.
		/// </summary>
		float BoughtPriceInEur { get; set; }

		/// <summary>
		/// Price for single unit in US dollars, when coins were bought.
		/// </summary>
		float BoughtPriceInUSD { get; set; }

		/// <summary>
		/// Only for representative purpose.
		/// </summary>
		string CoinName { get; set; }

		/// <summary>
		/// Used by other tools to find info about it on internet.
		/// </summary>
		string CoinUrlName { get; set; }

		/// <summary>
		/// Amount of current coins.
		/// </summary>
		double CurrentAmount { get; set; }

		/// <summary>
		/// Price for single unit in euros.
		/// </summary>
		float CurrentPriceInEur { get; set; }

		/// <summary>
		/// Price for single unit in US dollars.
		/// </summary>
		float CurrentPriceInUSD { get; set; }

		/// <summary>
		/// Updates current value properties for all available currencies.
		/// </summary>
		/// <returns>TRUE if success, otherwise FALSE.</returns>
		Task UpdateCurrentPrice();
	}
}
