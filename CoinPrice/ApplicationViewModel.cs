using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinPrice
{
	public class ApplicationViewModel : ObservableObject
	{
		public enum Status
		{
			/// <summary>
			/// Save already existing.
			/// </summary>
			Save,
			/// <summary>
			/// Save newly created.
			/// </summary>
			SaveNew,
			Cancel
		}

		//========================================================
		//	Legacy
		//========================================================

		#region Fields

		private ICommand _changePageCommand;

		private IPageViewModel _currentPageViewModel;
		private List<IPageViewModel> _pageViewModels;

		#endregion

		#region Properties / Commands

		public ICommand ChangePageCommand
		{
			get
			{
				if (_changePageCommand == null)
				{
					_changePageCommand = new RelayCommand(
						p => ChangeViewModel((IPageViewModel)p),
						p => p is IPageViewModel);
				}

				return _changePageCommand;
			}
		}

		public List<IPageViewModel> PageViewModels
		{
			get
			{
				if (_pageViewModels == null)
					_pageViewModels = new List<IPageViewModel>();

				return _pageViewModels;
			}
		}

		public IPageViewModel CurrentPageViewModel
		{
			get
			{
				return _currentPageViewModel;
			}
			set
			{
				if (_currentPageViewModel != value)
				{
					_currentPageViewModel = value;
					OnPropertyChanged("CurrentPageViewModel");
				}
			}
		}

		#endregion

		#region Methods

		private void ChangeViewModel(IPageViewModel viewModel)
		{
			if (!PageViewModels.Contains(viewModel))
				PageViewModels.Add(viewModel);

			CurrentPageViewModel = PageViewModels
				.FirstOrDefault(vm => vm == viewModel);
		}

		#endregion

		//========================================================
		//	Variables
		//========================================================

		private ICommand addCoinCommand;
		private ICommand modifyCoinCommand;
		private ICommand removeCoinCommand;

		private ContentViewModel content;
		private CoinEditViewModel coinEdit;

		private SQLiteDatabase database;
		private ICoinDataAccess coinAccess;

		private bool running;

		private UserCoinData currentlySelectedCoin;

		delegate void HandleEditCoinDel(Status status);

		//========================================================
		//	Constructors
		//========================================================

		public ApplicationViewModel()
		{
			running = true;

			database = new SQLiteDatabase();
			coinAccess = new CoinmarketcapAccess();

			var handleEditCoinDel = new HandleEditCoinDel(HandleEditCoin);

			App.Current.Exit += Current_Exit;

			content = new ContentViewModel();
			content.Coins = new ObservableCollection<UserCoinData>(database.GetAllCoins());
			Task.Run(() =>
			{
				while (running)
				{
					foreach (var coin in content.Coins)
					{
						coinAccess.UpdateCoinDataAsync(coin).Wait();
						RefreshCoin(coin);
					}
					Task.Delay(600000).Wait();
				}
			});
			coinEdit = new CoinEditViewModel(handleEditCoinDel, coinAccess);

			// Add available pages
			PageViewModels.Add(content);
			PageViewModels.Add(coinEdit);

			// Set starting page
			CurrentPageViewModel = content;
		}

		//========================================================
		//	Properties
		//========================================================

		public UserCoinData CurrentlySelectedCoin
		{
			get { return currentlySelectedCoin; }
			set
			{
				if (value != currentlySelectedCoin)
				{
					currentlySelectedCoin = value;
					OnPropertyChanged("CurrentlySelectedCoin");
				}
			}
		}

		//========================================================
		//	Commands
		//========================================================

		public ICommand AddCoinCommand
		{
			get
			{
				if (addCoinCommand == null)
				{
					addCoinCommand = new RelayCommand(
						param => AddCoin()
					);
				}
				return addCoinCommand;
			}
		}

		public ICommand ModifyCoinCommand
		{
			get
			{
				if (modifyCoinCommand == null)
				{
					modifyCoinCommand = new RelayCommand(
						param => ModifyCoin()
					);
				}
				return modifyCoinCommand;
			}
		}

		public ICommand RemoveCoinCommand
		{
			get
			{
				if (removeCoinCommand == null)
				{
					removeCoinCommand = new RelayCommand(
						param => RemoveCoin()
					);
				}
				return removeCoinCommand;
			}
		}

		//========================================================
		//	Handlers
		//========================================================

		private void Current_Exit(object sender, System.Windows.ExitEventArgs e)
		{
			running = false;
		}

		//========================================================
		//	Methods
		//========================================================
		//--------------------------------------------------------
		//	Private
		//--------------------------------------------------------

		private void RefreshCoin(UserCoinData coin)
		{
			// Following are set to trigger notifications that those properties have changed.
			coin.CurrentValueInEur = -1;
			coin.BoughtValueInEur = -1;
			coin.ValueChange = -1;
		}

		//--------------------------------------------------------
		//	Public
		//--------------------------------------------------------

		public void AddCoin()
		{
			coinEdit.UserCoin = null;
			CurrentPageViewModel = coinEdit;
		}

		public void ModifyCoin()
		{
			coinEdit.UserCoin = CurrentlySelectedCoin;
			CurrentPageViewModel = coinEdit;
		}

		public void RemoveCoin()
		{
			if (CurrentlySelectedCoin == null)
				return;
			database.RemoveCoin(CurrentlySelectedCoin);
			content.Coins.Remove(CurrentlySelectedCoin);
		}

		private async void HandleEditCoin(Status status)
		{
			var coin = coinEdit.UserCoin;
			if (status == Status.SaveNew)
			{
				if (database.AddCoin(coin))
				{
					content.Coins.Add(coin);
					CurrentPageViewModel = content;
					await coinAccess.UpdateCoinDataAsync(coin);
					RefreshCoin(coin);
				}
				else
				{
					Console.WriteLine("Failed to save coin to database.");
					// ERROR
				}
			}
			else if (status == Status.Save)
			{
				var update = database.UpdateCoin(coin);
				if (update)
				{
					CurrentPageViewModel = content;
					await coinAccess.UpdateCoinDataAsync(coin);
					RefreshCoin(coin);
				}
				else
				{
					Console.WriteLine("Failed to update coin to database.");
					// ERROR
				}
			}
			else if (status == Status.Cancel)
			{
				CurrentPageViewModel = content;
			}
		}
	}
}
