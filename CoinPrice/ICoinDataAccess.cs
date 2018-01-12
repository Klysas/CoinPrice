using System.Threading.Tasks;

namespace CoinPrice
{
	public interface ICoinDataAccess
	{
		/// <summary>
		/// Retrieves information about specific coin.
		/// </summary>
		/// <param name="coinName">Name of cryptocurrency coin</param>
		/// <returns></returns>
		Task<CoinJsonItem> GetCoinDataAsync(string coinName);

		/// <summary>
		/// Updates coin data(prices) for given userCoin.
		/// </summary>
		/// <param name="userCoin"></param>
		/// <returns></returns>
		Task<UserCoinData> UpdateCoinDataAsync(UserCoinData userCoin);
	}
}
