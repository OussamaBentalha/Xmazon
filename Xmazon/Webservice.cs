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
	public class Webservice
	{

		public Webservice (){}

		public async Task<JsonValue> httpRequest(string url, string method, HttpClient httpClient, FormUrlEncodedContent bodyContent) 
		{
			HttpResponseMessage response = null;
							
			switch (method) {
			case "GET":
				response = await httpClient.GetAsync (url);
				break;
			case "POST":
				response = await httpClient.PostAsync (url, bodyContent);
				break;
			case "PUT":
				response = await httpClient.PutAsync (url, bodyContent);
				break;
			case "DELETE":
				response = await httpClient.DeleteAsync (url);
				break;
			}

			if (response != null && response.StatusCode == HttpStatusCode.OK) {
				return JsonObject.Load(response.Content.ReadAsStreamAsync().Result);
			}

			if (response.StatusCode == HttpStatusCode.Unauthorized) {
				await AppContext.RefreshToken ();
				await UserContext.RefreshToken ();

				if (UserContext.AccessToken == null) {
					//TODO : go connexion page + message
					throw new UnauthorizedAccessException ("User token expired, need to re-connect.");
				}

				return await httpRequest(url, method, httpClient, bodyContent);
			}

			if (response.StatusCode >= HttpStatusCode.BadRequest) {
				Console.WriteLine (response.Content.ReadAsStringAsync ().Result);
			}

			return null;
		}


		public string encodeUrl(string url, Dictionary<string, string> urlParameters)
		{
			if (urlParameters == null) {
				return url;
			}
				
			string encodedUrl = string.Join(
				"&", 
				urlParameters.Select(param => { 
					return (param.Value != null) ? param.Key + "=" + Uri.EscapeDataString(param.Value) : "";
				})
			);

			return url + (string.IsNullOrEmpty(encodedUrl) ? "" : "?" + encodedUrl);
		}


		public FormUrlEncodedContent getHTTPBodyWithParameters(Dictionary<string, string> bodyParameters) 
		{
			if (bodyParameters == null) {
				return null;
			}

			return new FormUrlEncodedContent (bodyParameters);
		}


		public HttpClient setHTTPHeaderParameters(HttpClient httpClient, Dictionary<string, string> headers)
		{
			if (headers == null) {
				return null;
			}

			foreach (string headerKey in headers.Keys) {
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, headers[headerKey]);
			}
			return httpClient;
		}
	}
}