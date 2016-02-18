using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace Xmazon
{
	public static class ProductRepository
	{
		public static List<Product> Products { get; set; }

		public static async Task<List<Product>> list(string search, string categoryUid, string limit, string offset) 
		{
			Dictionary<string, string> getParameters = new Dictionary<string, string>();
			getParameters.Add ("search", search);
			getParameters.Add ("category_uid", categoryUid);
			getParameters.Add ("limit", limit);
			getParameters.Add ("offset", offset);

			if (UserContext.AccessToken == null) {
				return null;
			}

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + UserContext.AccessToken);

			XmazonRequest xmazon = new XmazonRequest ();
			JsonValue response;

			try {
				response = await xmazon.Call (
					XmazonRequest.PRODUCT_LIST, XmazonRequest.Method.GET,
					getParameters, null, headers);
			} catch(UnauthorizedAccessException ex) {
				throw ex;
			}

			JsonArray jsonProducts = (JsonArray) response ["result"];

			Products = new List<Product> ();

			foreach (JsonValue jsonProduct in jsonProducts) {
				Products.Add (Product.Deserialize (jsonProduct));
			}

			return Products;
		}
	}
}

