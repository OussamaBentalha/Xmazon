using System;
using System.Collections.Generic;
using System.Net.Http;

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
			var httpClient = new HttpClient ();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/auth/subscribe";
			var requestMethod = Webservice.Method.POST;


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);


			var bodyParameters = new Dictionary<string, string> ();
			bodyParameters.Add ("username", usernameEntry.Text);
			bodyParameters.Add ("password", passwordEntry.Text);
			bodyParameters.Add ("email", emailEntry.Text);
			bodyParameters.Add ("firstname", firstnameEntry.Text);
			bodyParameters.Add ("lastname", lastnameEntry.Text);
			var httpBodyContent = webservice.getHTTPBodyWithParameters (bodyParameters);

			var requestResult = await webservice.Call (url, requestMethod, httpClient, httpBodyContent);

			if (requestResult.ContainsKey ("result")) {
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
			}
			else {
				messageLabel.Text = "Sign up failed";
			}
		}

		bool AreDetailsValid (User user)
		{
			return (!string.IsNullOrWhiteSpace (user.Username) && !string.IsNullOrWhiteSpace (user.Password) && !string.IsNullOrWhiteSpace (user.Email) && user.Email.Contains ("@"));
		}
	}
}

