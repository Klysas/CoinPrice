using System.Windows;

namespace CoinPrice
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var app = new ApplicationView();
			var context = new ApplicationViewModel();
			app.DataContext = context;
			app.Show();
		}
	}
}
