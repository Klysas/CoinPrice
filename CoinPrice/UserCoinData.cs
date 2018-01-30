namespace CoinPrice
{
	/// <summary>
	/// Holds data for specific cryptocurrency coin.
	/// </summary>
	public class UserCoinData : ObservableObject
	{
		//========================================================
		//	Variables
		//========================================================
		//--------------------------------------------------------
		//	Private
		//--------------------------------------------------------

		private float boughtPriceInEur;
		private float boughtPriceInUSD;
		private string coinName;
		private string coinUrlName;
		private double currentAmount;
		private float currentPriceInEur;
		private float currentPriceInUSD;

		//========================================================
		//	Constructors
		//========================================================

		public UserCoinData()
		{
			// EMPTY
		}

		//========================================================
		//	Properties
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Price for single unit in euros, when coins were bought.
		/// </summary>
		public float BoughtPriceInEur
		{
			get { return boughtPriceInEur; }
			set
			{
				if (value != boughtPriceInEur)
				{
					boughtPriceInEur = value;
					OnPropertyChanged("BoughtPriceInEur");
				}
			}
		}

		/// <summary>
		/// Price for single unit in US dollars, when coins were bought.
		/// </summary>
		public float BoughtPriceInUSD
		{
			get { return boughtPriceInUSD; }
			set
			{
				if (value != boughtPriceInUSD)
				{
					boughtPriceInUSD = value;
					OnPropertyChanged("BoughtPriceInUSD");
				}
			}
		}

		/// <summary>
		/// Name for only representative purpose.
		/// </summary>
		public string CoinName
		{
			get { return coinName; }
			set
			{
				if (value != coinName)
				{
					coinName = value;
					OnPropertyChanged("CoinName");
				}
			}
		}

		/// <summary>
		/// Name for other tools to use on to find info about it on internet.
		/// </summary>
		public string CoinUrlName
		{
			get { return coinUrlName; }
			set
			{
				if (value != coinUrlName)
				{
					coinUrlName = value;
					OnPropertyChanged("CoinUrlName");
				}
			}
		}

		/// <summary>
		/// Amount of current coins.
		/// </summary>
		public double CurrentAmount
		{
			get { return currentAmount; }
			set
			{
				if (value != currentAmount)
				{
					currentAmount = value;
					OnPropertyChanged("CurrentAmount");
				}
			}
		}

		/// <summary>
		/// Price for single unit in euros.
		/// </summary>
		public float CurrentPriceInEur
		{
			get { return currentPriceInEur; }
			set
			{
				if (value != currentPriceInEur)
				{
					currentPriceInEur = value;
					OnPropertyChanged("CurrentPriceInEur");
				}
			}
		}

		/// <summary>
		/// Price for single unit in US dollars.
		/// </summary>
		public float CurrentPriceInUSD
		{
			get { return currentPriceInUSD; }
			set
			{
				if (value != currentPriceInUSD)
				{
					currentPriceInUSD = value;
					OnPropertyChanged("CurrentPriceInUSD");
				}
			}
		}

		//========================================================
		//	Methods
		//========================================================
		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public float GetBoughtValueInEur()
		{
			return (float)(BoughtPriceInEur * CurrentAmount);
		}

		public float GetCurrentValueInEur()
		{
			return (float)(CurrentPriceInEur * CurrentAmount);
		}

		// Is change different in USD than in EUR ?
		public float GetValueChange()
		{
			var change = GetCurrentValueInEur() / GetBoughtValueInEur();
			if (change >= 1) return change;
			change = 1 - change;
			return -change;
		}
	}
}
