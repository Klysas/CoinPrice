﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class ApplicationView : Window
	{
		public ApplicationView()
		{
			InitializeComponent();

		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ModifyCoin_Click(object sender, RoutedEventArgs e)
		{
			// TODO: Opens windows to modify(add/remove) coins.
		}
	}
}