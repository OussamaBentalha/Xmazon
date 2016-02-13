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
		}

		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			App.IsUserLoggedIn = false;
			Navigation.InsertPageBefore (new Connexion (), this);
			await Navigation.PopAsync ();
		}
	}
}

