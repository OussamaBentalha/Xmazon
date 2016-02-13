using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xmazon
{
	public partial class Register : ContentPage
	{

		public Register ()
		{	
			InitializeComponent ();
		}

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			var user = new User () {
				Username = usernameEntry.Text,
				Password = passwordEntry.Text,
				Email = emailEntry.Text,
				Firstname = firstnameEntry.Text,
				Lastname = LastnameEntry.Text,
				Birthdate = birthdayEntry.Text
			};


			var signUpSucceeded = AreDetailsValid (user);
			if (signUpSucceeded) {
				var rootPage = Navigation.NavigationStack [0];
				if (rootPage != null) {
					/*
			 		* 
			 		* Webservice d'inscription
					* 
			 		*/ 
					Mockups.user = user;
					App.IsUserLoggedIn = true;
					Navigation.InsertPageBefore (new ListStores (), Navigation.NavigationStack [0]);
					await Navigation.PopToRootAsync ();
				}
			} else {
				messageLabel.Text = "Sign up failed";
			}
		}

		bool AreDetailsValid (User user)
		{
			return (!string.IsNullOrWhiteSpace (user.Username) && !string.IsNullOrWhiteSpace (user.Password) && !string.IsNullOrWhiteSpace (user.Email) && user.Email.Contains ("@"));
		}
	}
}

