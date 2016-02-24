using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace Xmazon
{
	public class App : Application
	{

		public string AppToken = null;

		public App ()
		{
			MainPage = new NavigationPage(new Connexion());
		}
 
		protected override async void OnStart ()
		{
			await AppContext.RefreshToken ();
		}	

		public async Task<String> getAppToken(){
			string appToken = AppContext.AccessToken;
			if (String.IsNullOrEmpty (appToken)) {
				Console.WriteLine ("No token stored in local storage : " + appToken);
				System.Json.JsonValue response = await AppContext.RefreshToken();
				Console.WriteLine ("Refresh token response is : " + response.ToString());
				return await getAppToken ();
			} else {
				Console.WriteLine ("Token stored in local storage : " + appToken);
				return appToken;
			}
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

