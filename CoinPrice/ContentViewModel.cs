using System.Collections.Generic;

namespace CoinPrice
{
	public class ContentViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Variables
		//========================================================

		private List<UserCoinData> coins;

		//========================================================
		//	Constructors
		//========================================================

		public ContentViewModel()
		{
			Coins = new List<UserCoinData>();
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
