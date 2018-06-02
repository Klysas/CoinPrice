using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinPrice
{
	public class CoinEditViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Fields
		//========================================================

		private ICoinDataAccess coinAccess;
		private Delegate completeDelegate;
		private UserCoinData userCoin;

		private bool coinValid;
		private bool isNewCoin;

		private ICommand saveCommand;
		private ICommand cancelCommand;
		private ICommand validateCoinCommand;

		//========================================================
		//	Constructors
		//========================================================

		public CoinEditViewModel(Delegate completeDelegate, ICoinDataAccess coinAccess)
		{
			this.completeDelegate = completeDelegate;
			this.coinAccess = coinAccess;
			UserCoin = new UserCoinData();
			isNewCoin = false;
		}

		//========================================================
		//	Properties
		//========================================================

		public string Name
		{
			get
			{
				return "CoinEdit";
			}
		}

		public UserCoinData UserCoin
		{
			get { return userCoin; }
			set
			{
				if (value != userCoin)
				{
					userCoin = value;
					CoinValid = false;
					if (value == null)
						userCoin = new UserCoinData();
					if (userCoin.IsEmpty())
					{
						isNewCoin = true;
					}
					else
					{
						isNewCoin = false;
					}
					OnPropertyChanged("UserCoin");
				}
			}
		}

		public bool CoinValid
		{
			get { return coinValid; }
			set
			{
				if (value != coinValid)
				{
					coinValid = value;
					OnPropertyChanged("CoinValid");
				}
			}
		}

		//========================================================
		//	Commands
		//========================================================

		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
				{
					saveCommand = new RelayCommand(
						param => Save()
					);
				}
				return saveCommand;
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				if (cancelCommand == null)
				{
					cancelCommand = new RelayCommand(
						param => Cancel()
					);
				}
				return cancelCommand;
			}
		}

		public ICommand ValidateCoinCommand
		{
			get
			{
				if (validateCoinCommand == null)
				{
					validateCoinCommand = new RelayCommand(
						param => ValidateCoinUrl()
					);
				}
				return validateCoinCommand;
			}
		}

		//========================================================
		//	Methods
		//========================================================

		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		private void Save()
		{
			// TODO: validate all fields.
			if (isNewCoin)
			{
				completeDelegate.DynamicInvoke(ApplicationViewModel.Status.SaveNew);
			}
			else
			{
				completeDelegate.DynamicInvoke(ApplicationViewModel.Status.Save);
			}
			isNewCoin = false;
		}

		private void Cancel()
		{
			completeDelegate.DynamicInvoke(ApplicationViewModel.Status.Cancel);
		}

		private async Task<bool> IsCoinValid()
		{
			if (UserCoin.CoinUrlName.IsNullOrEmpty())
				return false;

			var result = await coinAccess.GetCoinDataAsync(UserCoin.CoinUrlName);

			if (!result.error.IsNull() && result.error.Equals("id not found"))
			{
				return false;
			}

			return true;
		}

		public async void ValidateCoinUrl()
		{
			CoinValid = await IsCoinValid();
		}
	}
}
