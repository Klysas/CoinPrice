using System.Collections.Generic;

namespace CoinPrice
{
	public class ContentViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Variables
		//========================================================

		private List<CoinModel> coins;

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

		public List<CoinModel> Coins
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
