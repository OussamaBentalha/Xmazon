using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;

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

			string url = XmazonRequest.CATEGORY_LIST;
			var method = XmazonRequest.Method.GET;

			var requestObject = new XmazonRequest ();

			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);


			var getParams = new Dictionary<string, string> ();
			getParams.Add ("store_uid", selectedStore.uid);

			var requestResult = await requestObject.Call (url, method, getParams, null, headers);

			if (requestResult.ContainsKey ("result")) {
				var jsonCategoriesList = requestResult ["result"];
				foreach (JsonValue category in jsonCategoriesList){
					Category currentCategory = Category.Deserialize(category);
					categoriesList.Add(currentCategory);
				}
				lstView.ItemsSource = categoriesList;
				lstView.ItemTemplate = new DataTemplate(typeof(TextCell));
				lstView.ItemTemplate.SetBinding(TextCell.TextProperty, "name");
				lstView.ItemSelected += async (sender, e) => {
					Category selectedCategory = (Category)e.SelectedItem;
					Navigation.InsertPageBefore (new ListProducts(selectedCategory), this);
					await Navigation.PopAsync ();
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

