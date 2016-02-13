using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xmazon
{
	public partial class ListViewXaml : ContentPage
	{
		public ListViewXaml ()
		{
			InitializeComponent ();
			lstView.ItemsSource = Mockups.stores;
			//lstView.ItemsSource = new List<string> () { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
		}
	}
}

