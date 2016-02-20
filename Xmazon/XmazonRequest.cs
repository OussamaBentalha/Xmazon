using System;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Xmazon
{
	public class XmazonRequest
	{
		public enum Method {
			GET,
			POST,
			PUT,
			DELETE
		};

		// ENDPOINT
		public const string SERVER_ENDPOINT = "http://xmazon.appspaces.fr";

		// RESOURCES
		public const string OAUTH_RESOURCE = SERVER_ENDPOINT + "/oauth";
		public const string AUTH_RESOURCE = SERVER_ENDPOINT + "/auth";
		public const string STORE_RESOURCE = SERVER_ENDPOINT + "/store";
		public const string CATEGORY_RESOURCE = SERVER_ENDPOINT + "/category";
		public const string PRODUCT_RESOURCE = SERVER_ENDPOINT + "/product";

		// WEB SERVICES
		public const string OAUTH_TOKEN = OAUTH_RESOURCE + "/token";

		public const string AUTH_SUBSCRIBE = AUTH_RESOURCE + "/subscribe";

		public const string STORE_LIST = STORE_RESOURCE + "/list";

		public const string CATEGORY_LIST = CATEGORY_RESOURCE + "/list";

		public const string PRODUCT_LIST = PRODUCT_RESOURCE + "/list";

		private HttpClient httpClient;

		public XmazonRequest ()
		{
			httpClient = new HttpClient ();
		}

		public async Task<JsonValue> Call(string url, Method method, 
			Dictionary<string, string> getParams, Dictionary<string, string> postParams, Dictionary<string, string> headers) 
		{
			string encodedUrl = GetUrlEncoded (url, getParams);

			HttpResponseMessage response = null;

			SetHeaders (headers);
				
			switch (method) {
			case Method.GET:
				response = await httpClient.GetAsync (encodedUrl);
				break;
			case Method.POST:
				response = await httpClient.PostAsync (encodedUrl, GetUrlEncodedBody (postParams));
				break;
			case Method.PUT:
				response = await httpClient.PutAsync (encodedUrl, GetUrlEncodedBody (postParams));
				break;
			case Method.DELETE:
				response = await httpClient.DeleteAsync (encodedUrl);
				break;
			}

			if (response != null && response.StatusCode == HttpStatusCode.OK) {
				return JsonObject.Load(response.Content.ReadAsStreamAsync().Result);
			}

			if (response.StatusCode == HttpStatusCode.Unauthorized) {
				await AppContext.RefreshToken ();
				await UserContext.RefreshToken ();

				if (UserContext.AccessToken == null) {
					throw new UnauthorizedAccessException ("User token expired, need to re-connect.");
				}

				return await Call(url, method, getParams, postParams, headers);
			}

			if (response.StatusCode >= HttpStatusCode.BadRequest) {
				Console.WriteLine (response.Content.ReadAsStringAsync ().Result);
			}

			return null;
		}


		private string GetUrlEncoded(string url, Dictionary<string, string> getParams)
		{
			if (getParams == null) {
				return url;
			}
				
			string getParamsEncoded = string.Join(
				"&", 
				getParams.Select(param => { 
					return (param.Value != null) ? param.Key + "=" + Uri.EscapeDataString(param.Value) : "";
				})
			);

			return url + (string.IsNullOrEmpty(getParamsEncoded) ? "" : "?" + getParamsEncoded);
		}


		private FormUrlEncodedContent GetUrlEncodedBody(Dictionary<string, string> postParams) 
		{
			if (postParams == null) {
				return null;
			}

			return new FormUrlEncodedContent (postParams);
		}


		private void SetHeaders(Dictionary<string, string> headers)
		{
			if (headers == null) {
				return;
			}

			foreach (string headerKey in headers.Keys) {
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, headers[headerKey]);
			}
		}
	}
}