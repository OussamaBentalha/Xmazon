using System;

using Xamarin.Forms;

namespace Xmazon
{
	public class App : Application
	{
		public static bool IsUserLoggedIn { get; set; }

		public App ()
		{
			if (!IsUserLoggedIn) {
				//MainPage = new NavigationPage (new LoginPage ());
			} else {
				//MainPage = new NavigationPage (new LoginNavigation.MainPage ());
			}
			// The root page of your application
			MainPage = new NavigationPage(new Connexion());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		public void connect(){
		}
	}
}

