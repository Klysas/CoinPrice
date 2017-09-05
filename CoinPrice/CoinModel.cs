namespace CoinPrice
{
	public class CoinModel : ObservableObject
	{
		//========================================================
		//	Variables
		//========================================================

		private string name;
		private string addressName;
		private double amount;

		//========================================================
		//	Constructors
		//========================================================

		public CoinModel()
		{
			Name = string.Empty;
			AddressName = string.Empty;
			Amount = 0;
		}

		//========================================================
		//	Properties
		//========================================================

		public string AddressName
		{
			get { return addressName; }
			set
			{
				if (value != addressName)
				{
					addressName = value;
					OnPropertyChanged("AddressName");
				}
			}
		}

		public double Amount
		{
			get { return amount; }
			set
			{
				if (value != amount)
				{
					amount = value;
					OnPropertyChanged("Amount");
				}
			}
		}

		public string Name
		{
			get { return name; }
			set
			{
				if (value != name)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}
	}
}
