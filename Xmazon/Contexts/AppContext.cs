﻿using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using System.Net;

namespace Xmazon
{
	public class AppContext
	{
		public const string CONTEXT_NAME = "app";
		public const string ACCESS_TOKEN = "access_token";
		public const string REFRESH_TOKEN = "refresh_token";

		public const string CLIENT_ID = "c2e1116c-182e-4fda-83ba-7fc17c61f81d";
		public const string CLIENT_SECRET = "48793a268ed2ca79031b31e3cab2d104193b1945";

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

		public static async Task<JsonValue> RefreshToken()
		{
			XmazonRequest xmazon = new XmazonRequest ();

			Dictionary<string, string> postParameters = new Dictionary<string, string> ();
			postParameters.Add ("client_id", CLIENT_ID);
			postParameters.Add ("client_secret", CLIENT_SECRET);

			if (Properties.ContainsKey(REFRESH_TOKEN)) {
				postParameters.Add ("grant_type", "refresh_token");
				postParameters.Add ("refresh_token", Properties [REFRESH_TOKEN] as string);
			} else {
				postParameters.Add ("grant_type", "client_credentials");
			}

			JsonValue response = await xmazon.Call (
				XmazonRequest.OAUTH_TOKEN, XmazonRequest.Method.POST,
				null, postParameters, null);

			if (response.ContainsKey("code")
				&& (int) response ["code"] >= 400
				&& (int) response ["code"] < 500) {
				Properties.Remove (ACCESS_TOKEN);
				Properties.Remove (REFRESH_TOKEN);
				return await RefreshToken ();
			}

			Properties [ACCESS_TOKEN] = (string) response [ACCESS_TOKEN];
			Properties [REFRESH_TOKEN] = (string) response [REFRESH_TOKEN];

			return response;
		}
	}
}
