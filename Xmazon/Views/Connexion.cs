using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;

namespace Xmazon
{
	public partial class Connexion : ContentPage
	{
		public Connexion ()
		{
			InitializeComponent();
			mailEntry.Text = "seb7@gmail.com";
			NavigationPage.SetHasBackButton(this, false);
		}	

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			await Navigation.PushAsync (new Register());
		}

		async void OnLoginButtonClicked (object sender, EventArgs e)
		{
			var result = await UserContext.Authenticate (mailEntry.Text, passwordEntry.Text);
			if (result != null){
				onConnexionSuccess ();
			} 
			else {
				onConnexionFailed ();
			}
		}

		void onConnexionSuccess(){
			messageLabel.Text = "";
			passwordEntry.Text = "";
			Navigation.PushAsync (new ListStores ());
		}

		void onConnexionFailed(){
			passwordEntry.Text = "";
			messageLabel.Text = "Pseudo/mot de passe incorrect.";
		}
	}
}

