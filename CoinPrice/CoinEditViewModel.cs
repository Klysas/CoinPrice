using System;
using System.Windows.Input;

namespace CoinPrice
{
	public class CoinEditViewModel : ObservableObject, IPageViewModel
	{
		//========================================================
		//	Fields
		//========================================================

		private UserCoinData userCoin;

		private Delegate completeDelegate;

		private ICommand saveCommand;
		private ICommand cancelCommand;

		//========================================================
		//	Constructors
		//========================================================

		public CoinEditViewModel(Delegate completeDelegate)
		{
			this.completeDelegate = completeDelegate;
			UserCoin = new UserCoinData();
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
					if (value == null)
						userCoin = new UserCoinData();
					OnPropertyChanged("UserCoin");
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

		//========================================================
		//	Methods
		//========================================================

		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		private void Save()
		{
			// TODO: validate all fields.
			completeDelegate.DynamicInvoke(ApplicationViewModel.Status.Save);
		}

		private void Cancel()
		{
			completeDelegate.DynamicInvoke(ApplicationViewModel.Status.Cancel);
		}

	}
}
