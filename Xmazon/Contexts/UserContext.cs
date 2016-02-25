using System;
using System.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Xmazon
{
	public class UserContext
	{
		public const string CONTEXT_NAME = "user";
		public const string ACCESS_TOKEN = "access_token";
		public const string REFRESH_TOKEN = "refresh_token";

		public static Dictionary<string, object> Properties { 
			get {
				if (!App.Current.Properties.ContainsKey (CONTEXT_NAME)) {
					App.Current.Properties [CONTEXT_NAME] = new Dictionary<string, object>();
				}

				return (Dictionary<string, object>) App.Current.Properties [CONTEXT_NAME];
			}
		}

		public static string AccessToken {
			get {
				if (!Properties.ContainsKey(ACCESS_TOKEN)) {
					return null;
				}

				return (string) Properties [ACCESS_TOKEN];
			}
		}

		public static async Task<JsonValue> Authenticate(string username, string password)
		{
			var httpClient = new HttpClient();
			var webservice = new Webservice ();
			var url = "http://xmazon.appspaces.fr/oauth/token"; 
			string requestMethod = "POST";

			Dictionary<string, string> bodyParameters = new Dictionary<string, string> ();
			bodyParameters.Add ("grant_type", "password");
			bodyParameters.Add ("client_id", AppContext.CLIENT_ID);
			bodyParameters.Add ("client_secret", AppContext.CLIENT_SECRET);
			bodyParameters.Add ("username", username);
			bodyParameters.Add ("password", password);

			var bodyContent = webservice.getHTTPBodyWithParameters (bodyParameters);
		
			JsonValue jsonToken = await webservice.httpRequest (url, requestMethod, httpClient, bodyContent);
			if (jsonToken != null) {
				Properties [ACCESS_TOKEN] = (string)jsonToken [ACCESS_TOKEN];
				Properties [REFRESH_TOKEN] = (string)jsonToken [REFRESH_TOKEN];
				return jsonToken;
			}
			else {
				return null;
			}
		}

		public static async Task<JsonValue> RefreshToken()
		{
			var httpClient = new HttpClient ();
			Webservice webservice = new Webservice ();
			var url = "http://xmazon.appspaces.fr/oauth/token"; 
			string requestMethod = "POST";

			Dictionary<string, string> bodyParameters = new Dictionary<string, string> ();
			bodyParameters.Add ("grant_type", "refresh_token");
			bodyParameters.Add ("client_id", AppContext.CLIENT_ID);
			bodyParameters.Add ("client_secret", AppContext.CLIENT_SECRET);
			bodyParameters.Add ("refresh_token", Properties [REFRESH_TOKEN] as string);
			var bodyContent = webservice.getHTTPBodyWithParameters (bodyParameters);

			JsonValue response = await webservice.httpRequest (url, requestMethod, httpClient, bodyContent);

			if (response.ContainsKey("code") && (int) response ["code"] < 400) {
				Properties [ACCESS_TOKEN] = (string) response [ACCESS_TOKEN];
				Properties [REFRESH_TOKEN] = (string) response [REFRESH_TOKEN];
			} else {
				Properties.Remove (ACCESS_TOKEN);
				Properties.Remove (REFRESH_TOKEN);
			}

			return response;
		}
	}
}

