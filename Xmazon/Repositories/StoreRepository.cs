using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace Xmazon
{
	public static class StoreRepository
	{
		public static List<Store> Stores { get; set; }

		public static async Task<List<Store>> list() 
		{
			if (AppContext.AccessToken == null) {
				await AppContext.RefreshToken ();
			}

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", "Bearer " + AppContext.AccessToken);

			XmazonRequest xmazon = new XmazonRequest ();

			JsonValue response;

			try {
				response = await xmazon.Call (
					XmazonRequest.STORE_LIST, XmazonRequest.Method.GET,
					null, null, headers);
			} catch(UnauthorizedAccessException ex) {
				throw ex;
			}

			JsonArray jsonStores = (JsonArray) response ["result"];
		
			Stores = new List<Store> ();

			foreach (JsonValue jsonStore in jsonStores) {
				Stores.Add (Store.Deserialize (jsonStore));
			}
				
			return Stores;
		}
	}
}

