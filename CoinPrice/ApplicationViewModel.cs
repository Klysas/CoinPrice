using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinPrice
{
	public class ApplicationViewModel : ObservableObject
	{
		public enum Status
		{
			Save,
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

		private ContentViewModel content;
		private CoinEditViewModel coinEdit;

		private SQLiteDatabase database;
		private ICoinDataAccess coinAccess;

		private bool running;

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
			content.Coins = database.GetAllCoins();
			Task.Run(() =>
			{
				while (running)
				{
					foreach (var coin in content.Coins)
					{
						coinAccess.UpdateCoinDataAsync(coin).Wait();

						// Following are set to trigger notifications that those properties have changed.
						coin.CurrentValueInEur = -1;
						coin.BoughtValueInEur = -1;
						coin.ValueChange = -1;
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

		public void AddCoin()
		{
			coinEdit.UserCoin = null;
			CurrentPageViewModel = coinEdit;
		}

		private async void HandleEditCoin(Status status)
		{
			if (status == Status.Save)
			{
				if (database.AddCoin(coinEdit.UserCoin))
				{
					content.Coins.Add(coinEdit.UserCoin);
					CurrentPageViewModel = content;
					await coinAccess.UpdateCoinDataAsync(coinEdit.UserCoin);
				}
				else
				{
					Console.WriteLine("Failed to save coin to database.");
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
