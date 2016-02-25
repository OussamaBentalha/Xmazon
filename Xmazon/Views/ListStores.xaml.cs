using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;
using System.Net.Http;

namespace Xmazon
{
	public partial class ListStores : ContentPage
	{
		private ArrayList storesList = new ArrayList();
		public ListStores ()
		{
			InitializeComponent ();
			initializeStoresList ();
		}

		private async void initializeStoresList(){
			var httpClient = new HttpClient ();
			Webservice webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/store/list";
			string requestMethod = "GET";


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);

			var bodyContent = webservice.getHTTPBodyWithParameters(null);

			var requestResult = await webservice.httpRequest (url, requestMethod, httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				var jsonStoresList = requestResult ["result"];
				foreach (JsonValue store in jsonStoresList){
					Store currentStore = Store.Deserialize(store);
					storesList.Add(currentStore);
				}
				lstView.ItemsSource = storesList;
				lstView.ItemSelected += async (sender, e) => {
					Store selectedStore = (Store)e.SelectedItem;
					Navigation.PushAsync (new ListCategories(selectedStore));
				};
			}
		}


		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			/*
			 * 
			 * Webservice de déconnexion
			 * 
			 */ 
			//App.IsUserLoggedIn = false;
			Navigation.InsertPageBefore (new Connexion (), this);
			await Navigation.PopAsync ();
		}
	}
}

