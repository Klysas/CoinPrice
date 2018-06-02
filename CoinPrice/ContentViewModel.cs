using System.Collections.ObjectModel;

namespace CoinPrice
{
	public class ContentViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Variables
		//========================================================

		private ObservableCollection<UserCoinData> coins;

		//========================================================
		//	Constructors
		//========================================================

		public ContentViewModel()
		{
			coins = new ObservableCollection<UserCoinData>();
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

		public ObservableCollection<UserCoinData> Coins
		{
			get { return coins; }
			set
			{
				if (value != coins)
				{
					if (value != null)
					{
						foreach (var coin in value)
						{
							coins.Add(coin);
						}
					}
					OnPropertyChanged("Coins");
				}
			}
		}
	}
}
