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
			string requestMethod = "GET";


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);


			var urlParameters = new Dictionary<string, string> ();
			urlParameters.Add ("category_uid", selectedCategory.uid);
			url = webservice.encodeUrl(url, urlParameters);

			var bodyContent = webservice.getHTTPBodyWithParameters(null);


			var requestResult = await webservice.httpRequest (url, requestMethod,httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				var jsonProductsList = requestResult ["result"];
				foreach (JsonValue product in jsonProductsList){
					Product currentProduct = Product.Deserialize(product);
					productsList.Add(currentProduct);
				}
				lstView.ItemsSource = productsList;
				lstView.ItemSelected += async (sender, e) => {
					Product selectedProduct = (Product)e.SelectedItem;
					OnActionSheetSimpleClicked (sender, e, selectedProduct);
				};

			}
		}

		void OpenCart (object sender, EventArgs e)
		{
			Navigation.PushAsync (new CartView());	
		}

		async void OnActionSheetSimpleClicked (object sender, EventArgs e, Product selectedProduct)
		{	
			var action = await DisplayActionSheet ("Produit: Ajouter au panier?", "Annuler", null, "Ajouter");
			if (action == "Ajouter") {
				Console.WriteLine ("Action: " + action);
				addProductToCart (selectedProduct);	
			}	
		}

		async void addProductToCart(Product selectedProduct){
			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/cart/add";
			string requestMethod = "PUT";


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);

			var bodyParameters = new Dictionary<string, string> ();
			bodyParameters.Add ("product_uid", selectedProduct.uid);
			bodyParameters.Add ("quantity", "1");
			var bodyContent = webservice.getHTTPBodyWithParameters (bodyParameters);



			var requestResult = await webservice.httpRequest (url, requestMethod,httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				DisplayAlert ("Confirmation", "Produit ajouté.", "Fermer");
			}
			else {
				DisplayAlert ("Erreur", "Produit non ajouté.", "Fermer");
			}
		}
			
		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			Navigation.InsertPageBefore (new Connexion (), this);
		}
	}
}

