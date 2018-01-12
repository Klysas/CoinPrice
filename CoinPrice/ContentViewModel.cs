using System.Collections.Generic;

namespace CoinPrice
{
	public class ContentViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Variables
		//========================================================

		private List<UserCoinData> coins;

		// REMOVE
		public ContentViewModel()
		{
			Coins = new List<UserCoinData>();

			Coins.Add(new UserCoinData(new CoinmarketcapAccess()) { CoinName = "Bitcoin", CoinUrlName = "bitcoin", CurrentAmount = 0.2});
		}

		//========================================================
		//	Properties
		//========================================================

		public string Name
		{
			get
			{
				return "Content";
			}
		}

		public List<UserCoinData> Coins
		{
			get { return coins; }
			set
			{
				if (value != coins)
				{
					coins = value;
					OnPropertyChanged("Coins");
				}
			}
		}
	}
}
