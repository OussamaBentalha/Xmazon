using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace Xmazon
{
	public static class CategoryRepository
	{
		public static List<Category> Categories { get; set; }

		public static async Task<List<Category>> list(string search, string storeUid, string limit, string offset) 
		{
			Dictionary<string, string> getParameters = new Dictionary<string, string>();
			getParameters.Add ("search", search);
			getParameters.Add ("store_uid", storeUid);
			getParameters.Add ("limit", limit);
			getParameters.Add ("offset", offset);

			if (AppContext.AccessToken == null) {
				await AppContext.RefreshToken ();
			}

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);

			XmazonRequest xmazon = new XmazonRequest ();

			JsonValue response;

			try {
				response = await xmazon.Call (
					XmazonRequest.CATEGORY_LIST, XmazonRequest.Method.GET, 
					getParameters, null, headers);
			} catch(UnauthorizedAccessException ex) {
				throw ex;
			}

			JsonArray categoriesJson = (JsonArray) response ["result"];

			Categories = new List<Category> ();

			foreach (JsonValue categoryJson in categoriesJson) {
				Categories.Add (Category.Deserialize (categoryJson));
			}

			return Categories;
		}
	}
}

