namespace CoinPrice
{
	public class UserCoinViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Variables
		//========================================================

		private UserCoinData currentUserCoin;

		//========================================================
		//	Properties
		//========================================================

		public string Name
		{
			get
			{
				return "UserCoin";
			}
		}

		public UserCoinData CurrentUserCoin
		{
			get { return currentUserCoin; }
			set
			{
				if (value != currentUserCoin)
				{
					currentUserCoin = value;
					OnPropertyChanged("CurrentUserCoin");
				}
			}
		}
	}
}
