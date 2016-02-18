using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xmazon
{
	public partial class ListStores : ContentPage
	{
		public ListStores ()
		{
			InitializeComponent ();
			lstView.ItemsSource = Mockups.stores;
		}

		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			/*
			 * 
			 * Webservice de déconnexion
			 * 
			 */ 
			//App.IsUserLoggedIn = false;
			Navigation.InsertPageBefore (new Connexion (), this);
			await Navigation.PopAsync ();
		}
	}
}

