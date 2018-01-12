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
	/// Interaction logic for CoinEditView.xaml
	/// </summary>
	public partial class CoinEditView : UserControl
	{
		private bool coinUrlNameBoxModified = false;

		public CoinEditView()
		{
			InitializeComponent();
		}

		private void CoinNameBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!coinUrlNameBoxModified)
			{
				CoinUrlNameBox.Text = CoinNameBox.Text.ToLower();
			}
		}

		private void CoinUrlNameBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!CoinUrlNameBox.Text.Equals(CoinNameBox.Text.ToLower())) coinUrlNameBoxModified = true;
		}
	}
}
