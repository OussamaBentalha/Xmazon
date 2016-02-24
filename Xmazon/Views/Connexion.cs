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
			usernameEntry.Text = "seb7@gmail.com";
			passwordEntry.Text = "";
		}	

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			//TODO launch Register()
			await Navigation.PushAsync (new Register());
		}

		async void OnLoginButtonClicked (object sender, EventArgs e)
		{
			
			var result = await UserContext.Authenticate (usernameEntry.Text, passwordEntry.Text);
			if (result != null){
				Console.WriteLine ("Connexion reussie : " + result["access_token"]);
				messageLabel.Text = "";
				passwordEntry.Text = "";
				Navigation.PushAsync (new ListStores ());
				
			} else {
				passwordEntry.Text = "";
				messageLabel.Text = "Pseudo/mot de passe incorrect.";
			}
		}

		bool AreCredentialsCorrect (User user)
		{
			/*
			 * 
			 * Webservice de connexion
			 * 
			 */ 
			return true; 
			//if(Mockups.user != null) return user.Username == Mockups.user.Username && user.Password == Mockups.user.Password;
			//return false;
		}

	}
}

