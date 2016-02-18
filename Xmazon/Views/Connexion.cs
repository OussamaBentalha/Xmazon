using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xmazon
{
	public partial class Connexion : ContentPage
	{
		public Connexion ()
		{
			InitializeComponent();
		}

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			//TODO launch Register()
			await Navigation.PushAsync (new Register());
		}

		async void OnLoginButtonClicked (object sender, EventArgs e)
		{

			var user = new User {
				Username = usernameEntry.Text,
				Password = passwordEntry.Text
			};

			var isValid = AreCredentialsCorrect (user);
			if (isValid) {
				//	App.IsUserLoggedIn = true;
				Navigation.InsertPageBefore (new ListStores (), this);
				await Navigation.PopAsync ();
			} else {
				messageLabel.Text = "Login failed";
				passwordEntry.Text = string.Empty;
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

