using System;
using System.Collections.Generic;
using System.Json;
using Xamarin.Forms;
using System.Collections;
using System.Net.Http;

namespace Xmazon
{
	public partial class CartView : ContentPage
	{
		private ArrayList cartProducts = new ArrayList();

		public CartView ()
		{
			InitializeComponent ();
			initializeProductsList ();
		}

		private async void initializeProductsList(){

			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string requestMethod = "GET";
			string url = "http://xmazon.appspaces.fr/cart";

			var userToken = UserContext.AccessToken;
			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);
		

			var bodyContent = webservice.getHTTPBodyWithParameters(null);

			var requestResult = await webservice.httpRequest (url, requestMethod, httpClient, bodyContent);

			if (requestResult.ContainsKey ("result") && requestResult ["result"].ContainsKey("products_cart")) {
				var jsonProductsList = requestResult ["result"]["products_cart"];
				foreach (JsonValue product in jsonProductsList){
					CartProduct currentProduct = CartProduct.Deserialize(product);
					cartProducts.Add(currentProduct);
				}
				lstView.ItemsSource = cartProducts;
				lstView.ItemSelected += async (sender, e) => {
					CartProduct selectedProduct = (CartProduct)e.SelectedItem;
					OnActionSheetSimpleClicked(sender, e, selectedProduct);
				};
			}
		}


		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			Navigation.InsertPageBefore (new Connexion (), this);
			await Navigation.PopAsync ();
		}


		void OnOrderButtonClicked (object sender, EventArgs e)
		{
			sendOrder();
		}

		private async void sendOrder(){
			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/order/create";
			string requestMethod = "PUT";


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);

			var bodyContent = webservice.getHTTPBodyWithParameters (null);

	
			var requestResult = await webservice.httpRequest (url, requestMethod,httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				DisplayAlert ("Confirmation", "Merci pour votre commande.", "Fermer");
			}
			else {
				DisplayAlert ("Erreur", "Impossible d'envoyer la commande.", "Fermer");
			}
		}
		private async void OnActionSheetSimpleClicked (object sender, EventArgs e, CartProduct selectedCartProduct)
		{
			var action = await DisplayActionSheet ("Supprimer le produit ? ", "Non", null, "Oui");
			Console.WriteLine ("Action: " + action);
			if (action == "Oui") {
				deleteProductFromCart (selectedCartProduct); 
			}	
		}

		private async void deleteProductFromCart(CartProduct selectedCartProduct){
			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			string url = "http://xmazon.appspaces.fr/cart/remove";
			string requestMethod = "DELETE";


			var headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);
			httpClient = webservice.setHTTPHeaderParameters (httpClient, headers);

			var bodyParameters = new Dictionary<string, string> ();
			bodyParameters.Add ("product_uid", selectedCartProduct.uid);
			bodyParameters.Add ("quantity", "1");
			var bodyContent = webservice.getHTTPBodyWithParameters (bodyParameters);



			var requestResult = await webservice.httpRequest (url, requestMethod,httpClient, bodyContent);

			if (requestResult.ContainsKey ("result")) {
				DisplayAlert ("Confirmation", "Produit supprimé.", "Fermer");
				var JSONCartProducts = requestResult ["result"];
				cartProducts = new ArrayList();
				foreach (JsonValue cartProduct in JSONCartProducts) {
					CartProduct currentCartProduct = CartProduct.Deserialize(cartProduct);
					cartProducts.Add(currentCartProduct);
				}
				lstView.ItemsSource = cartProducts;
				lstView.ItemSelected += async (sender, e) => {
					CartProduct selectedProduct = (CartProduct)e.SelectedItem;
					OnActionSheetSimpleClicked (sender, e, selectedProduct);
				};
			}
			else {
				DisplayAlert ("Erreur", "Produit non supprimé.", "Fermer");
			}
		}

		async void OpenCart (object sender, EventArgs e)
		{
			Navigation.PushAsync (new CartView());	
		}
	}
}