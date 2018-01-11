using System;
using System.Windows;

namespace CoinPrice
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class ApplicationView : Window
	{
		IUserCoinData bitcoin;

		public ApplicationView()
		{
			InitializeComponent();

			bitcoin = new UserCoinData(new CoinmarketcapAccess())
			{
				CoinName = "BitCoin",
				CoinUrlName = "bitcoin",
				CurrentAmount = 0.2,
				BoughtPriceInEur = 2300.2f
			};
			

		}

		public float GetCurrentValueInEur()
		{
			return (float)(bitcoin.CurrentPriceInEur * bitcoin.CurrentAmount);
		}

		public float GetBoughtValueInEur()
		{
			return (float)(bitcoin.BoughtPriceInEur * bitcoin.CurrentAmount);
		}

		public float GetValueChange()
		{
			var change = GetCurrentValueInEur() / GetBoughtValueInEur();
			if (change >= 1) return change;
			change = 1 - change;
			return -change;
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			await bitcoin.UpdateCurrentPrice();
			Console.WriteLine("{0} {1} {2}  |  {3} {4} {5:P}", bitcoin.CurrentAmount, bitcoin.BoughtPriceInEur, bitcoin.CurrentPriceInEur, GetCurrentValueInEur(), GetBoughtValueInEur(), GetValueChange());
		}

		private void ModifyCoin_Click(object sender, RoutedEventArgs e)
		{
			// TODO: Opens windows to modify(add/remove) coins.
		}
	}
}
