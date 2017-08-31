using System.Threading.Tasks;

namespace CoinPrice
{
	interface ICoinDataAccess
	{
		/// <summary>
		/// Retrieves information about specific coin.
		/// </summary>
		/// <param name="coinName">Name of cryptocurrency coin</param>
		/// <returns></returns>
		Task<CoinJsonItem> GetCoinDataAsync(string coinName);
	}
}
