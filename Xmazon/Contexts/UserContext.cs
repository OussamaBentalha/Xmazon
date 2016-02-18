using System;
using System.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

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
			XmazonRequest xmazon = new XmazonRequest ();

			Dictionary<string, string> postParameters = new Dictionary<string, string> ();
			postParameters.Add ("grant_type", "password");
			postParameters.Add ("client_id", AppContext.CLIENT_ID);
			postParameters.Add ("client_secret", AppContext.CLIENT_SECRET);
			postParameters.Add ("username", username);
			postParameters.Add ("password", password);

			JsonValue jsonToken = await xmazon.Call (
				XmazonRequest.OAUTH_TOKEN, XmazonRequest.Method.POST, 
				null, postParameters, null);

			Properties [ACCESS_TOKEN] = (string) jsonToken [ACCESS_TOKEN];
			Properties [REFRESH_TOKEN] = (string) jsonToken [REFRESH_TOKEN];

			return jsonToken;
		}

		public static async Task<JsonValue> RefreshToken()
		{
			XmazonRequest xmazon = new XmazonRequest ();

			Dictionary<string, string> postParameters = new Dictionary<string, string> ();
			postParameters.Add ("grant_type", "refresh_token");
			postParameters.Add ("client_id", AppContext.CLIENT_ID);
			postParameters.Add ("client_secret", AppContext.CLIENT_SECRET);
			postParameters.Add ("refresh_token", Properties [REFRESH_TOKEN] as string);

			JsonValue response = await xmazon.Call (
				XmazonRequest.OAUTH_TOKEN, XmazonRequest.Method.POST, 
				null, postParameters, null);

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

