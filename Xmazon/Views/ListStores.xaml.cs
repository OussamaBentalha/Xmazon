using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;

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
			string url = XmazonRequest.STORE_LIST;
			var method = XmazonRequest.Method.GET;

			var requestObject = new XmazonRequest ();
			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);
			var requestResult = await requestObject.Call (url, method, null, null, headers);
			if (requestResult.ContainsKey ("result")) {
				var jsonStoresList = requestResult ["result"];
				foreach(JsonValue store in jsonStoresList){
					Store currentStore = Store.Deserialize (store);
					storesList.Add(currentStore);
				}
				lstView.ItemsSource = storesList;
				lstView.ItemTemplate = new DataTemplate(typeof(TextCell));
				lstView.ItemTemplate.SetBinding(TextCell.TextProperty, "name");
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

