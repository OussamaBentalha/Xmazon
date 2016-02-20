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

			string url = XmazonRequest.AUTH_SUBSCRIBE;
			var method = XmazonRequest.Method.POST;

			var requestObject = new XmazonRequest ();

			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);


			var postParams = new Dictionary<string, string> ();
			postParams.Add ("username", usernameEntry.Text);
			postParams.Add ("password", passwordEntry.Text);
			postParams.Add ("email", emailEntry.Text);
			postParams.Add ("firstname", firstnameEntry.Text);
			postParams.Add ("lastname", lastnameEntry.Text);

			var requestResult = await requestObject.Call (url, method, null, postParams, headers);

			if (requestResult.ContainsKey ("result")) {
				Console.Write ("ok");
			}
		
			var signUpSucceeded = true; //AreDetailsValid (user);
			if (signUpSucceeded) {
				var rootPage = Navigation.NavigationStack [0];
				if (rootPage != null) {
					/*
			 		* 
			 		* Webservice d'inscription
					* 
			 		*/ 
				//	Mockups.user = user;
					//App.IsUserLoggedIn = true;
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

