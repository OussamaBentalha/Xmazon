using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Xmazon
{
	public class Request
	{
		public Request ()
		{
			
		}

		public string HttpPostRequest(string url, Dictionary<string,string> postParameters)
		{
			url = "http://Mydomain:3000/" + url;
			string postData = "";
			foreach (string key in postParameters.Keys)
			{
				postData += HttpUtility.UrlEncode(key) + "="
					+ HttpUtility.UrlEncode(postParameters[key]) + "&";
			}

			HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
			myHttpWebRequest.Method = "POST";
			byte[] data = System.Text.Encoding.ASCII.GetBytes(postData);
			myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
			myHttpWebRequest.ContentLength = data.Length;
			Stream requestStream = myHttpWebRequest.GetRequestStream();
			requestStream.Write(data, 0, data.Length);
			requestStream.Close();
			HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream responseStream = myHttpWebResponse.GetResponseStream();
			StreamReader myStreamReader = new StreamReader(responseStream, System.Text.Encoding.Default);
			string pageContent = myStreamReader.ReadToEnd();
			myStreamReader.Close();
			responseStream.Close();
			myHttpWebResponse.Close();
			return pageContent;
		}
	}
}

