using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinPrice
{
	/// <summary>
	/// Interaction logic for UserCoinView.xaml
	/// </summary>
	public partial class UserCoinView : UserControl
	{
		public UserCoinView()
		{
			InitializeComponent();
		}

		private void DockPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Change to capturer view
			var applicationViewModel = App.Current.MainWindow.DataContext as ApplicationViewModel;
			if (applicationViewModel.IsNull())
			{
				// ERROR
			}
			applicationViewModel.CurrentlySelectedCoin = ((DockPanel)sender).DataContext as UserCoinData;
		}
	}
}
