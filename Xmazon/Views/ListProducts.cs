using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;
using System.Net.Http;

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
			
			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/product/list";
			var requestMethod = Webservice.Method.GET;


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);


			var urlParameters = new Dictionary<string, string> ();
			urlParameters.Add ("category_uid", selectedCategory.uid);
			url = webservice.encodeUrl(url, urlParameters);

			var bodyContent = webservice.getHTTPBodyWithParameters(null);


			var requestResult = await webservice.Call (url, requestMethod,httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				var jsonProductsList = requestResult ["result"];
				foreach (JsonValue product in jsonProductsList){
					Product currentProduct = Product.Deserialize(product);
					productsList.Add(currentProduct);
				}
				lstView.ItemsSource = productsList;
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
		}
	}
}

