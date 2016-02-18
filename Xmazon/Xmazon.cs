using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace Xmazon
{
	public class App : Application
	{
		public App ()
		{
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
	}
}

