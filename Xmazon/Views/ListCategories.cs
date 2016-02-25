using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;
using System.Net.Http;

namespace Xmazon
{
	public partial class ListCategories : ContentPage
	{
		private ArrayList categoriesList = new ArrayList();
		private Store selectedStore;

		public ListCategories (Store store)
		{
			selectedStore = store;

			InitializeComponent ();
			initializeStoresList ();
		}

		private async void initializeStoresList(){

			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/category/list";
			var requestMethod = Webservice.Method.GET;

			var requestObject = new Webservice ();

			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);

			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);

			var urlParameters = new Dictionary<string, string> ();
			urlParameters.Add ("store_uid", selectedStore.uid);

			url = webservice.encodeUrl (url, urlParameters);

			var bodyContent = webservice.getHTTPBodyWithParameters(null);

				
			var requestResult = await requestObject.Call (url, requestMethod, httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				var jsonCategoriesList = requestResult ["result"];
				foreach (JsonValue category in jsonCategoriesList){
					Category currentCategory = Category.Deserialize(category);
					categoriesList.Add(currentCategory);
				}
				lstView.ItemsSource = categoriesList;
				lstView.ItemSelected += async (sender, e) => {
					Category selectedCategory = (Category)e.SelectedItem;
					Navigation.PushAsync (new ListProducts(selectedCategory));
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

