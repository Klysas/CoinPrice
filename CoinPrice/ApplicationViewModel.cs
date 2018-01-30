using System;
using System.Collections.Generic;
using System.Linq;
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

		delegate void HandleEditCoinDel(Status status);

		//========================================================
		//	Constructors
		//========================================================

		public ApplicationViewModel()
		{
			database = new SQLiteDatabase();

			var handleEditCoinDel = new HandleEditCoinDel(HandleEditCoin);

			content = new ContentViewModel();
			content.Coins = database.GetAllCoins();
			coinEdit = new CoinEditViewModel(handleEditCoinDel);

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
		//	Methods
		//========================================================

		public void AddCoin()
		{
			coinEdit.UserCoin = null;
			CurrentPageViewModel = coinEdit;
		}

		private void HandleEditCoin(Status status)
		{
			if (status == Status.Save)
			{
				if (database.AddCoin(coinEdit.UserCoin))
				{
					content.Coins.Add(coinEdit.UserCoin);
					CurrentPageViewModel = content;
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
