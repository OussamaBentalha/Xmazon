using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;

namespace Xmazon
{
	public partial class ListProducts : ContentPage
	{
		private ArrayList productsList = new ArrayList();
		private Category selectedCategory;

		public ListProducts (Category category)
		{
			selectedCategory = category;
			InitializeComponent();
			initializeProductsList ();
		}

		private async void initializeProductsList(){

			string url = XmazonRequest.PRODUCT_LIST;
			var method = XmazonRequest.Method.GET;

			var requestObject = new XmazonRequest ();

			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);


			var getParams = new Dictionary<string, string> ();
			getParams.Add ("category_uid", selectedCategory.uid);

			var requestResult = await requestObject.Call (url, method, getParams, null, headers);

			if (requestResult.ContainsKey ("result")) {
				var jsonProductsList = requestResult ["result"];
				foreach (JsonValue product in jsonProductsList){
					Product currentProduct = Product.Deserialize(product);
					productsList.Add(currentProduct);
				}
				lstView.ItemsSource = productsList;
				lstView.ItemTemplate = new DataTemplate(typeof(TextCell));

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

