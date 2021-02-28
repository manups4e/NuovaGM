using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server
{
	public struct RequestResponse
	{
		public HttpStatusCode status;
		public WebHeaderCollection headers;
		public string content;
	}

	public struct RequestDataInternal
	{
		public string url;
		public string method;
		public string data;
		public dynamic headers;
	}

	public static class RequestInternal
	{
		public static ConcurrentDictionary<int, ConcurrentDictionary<string, dynamic>> responseDictionary;

		public static void Init()
		{
			responseDictionary = new ConcurrentDictionary<int, ConcurrentDictionary<string, dynamic>>();
			Server.Instance.AddEventHandler("__cfx_internal:httpResponse", new Action<int, int, string, dynamic>(Response));
		}

		public static void Response(int token, int status, string text, dynamic header)
		{
			ConcurrentDictionary<string, dynamic> response = new ConcurrentDictionary<string, dynamic>();
			response["headers"] = header;
			response["status"] = status;
			response["content"] = text;
			responseDictionary[token] = response;
		}

		public static async Task<ConcurrentDictionary<string, dynamic>> Http(string url, string method, string data, dynamic headers)
		{
			RequestDataInternal requestData = new RequestDataInternal();
			requestData.url = url;
			requestData.method = method;
			requestData.data = data;
			requestData.headers = headers;
			string json = requestData.Serialize();
			int token = API.PerformHttpRequestInternal(json, Encoding.UTF8.GetByteCount(json));
			while (!responseDictionary.ContainsKey(token)) await BaseScript.Delay(0);
			ConcurrentDictionary<string, dynamic> res = responseDictionary[token];
			responseDictionary.TryRemove(token, out res);

			return res;
		}
	}

	public static class Request
	{
		public static async Task<RequestResponse> Http(string url, string method = "GET", string data = "", ConcurrentDictionary<string, string> headers = null)
		{
			headers = headers == null ? new ConcurrentDictionary<string, string>() : headers;

			return ParseRequestResponseInternal(await RequestInternal.Http(url, method, data, headers));
		}

		private static WebHeaderCollection ParseHeadersInternal(dynamic headerDyn)
		{
			WebHeaderCollection headers = new WebHeaderCollection();
			IDictionary<string, object> headerDict = (IDictionary<string, object>)headerDyn;
			foreach (KeyValuePair<string, object> entry in headerDict) headers.Add(entry.Key, entry.Value.ToString());

			return headers;
		}

		private static HttpStatusCode ParseStatusInternal(int status) { return (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), status); }

		private static RequestResponse ParseRequestResponseInternal(IDictionary<string, dynamic> rr)
		{
			RequestResponse result = new RequestResponse();
			result.status = ParseStatusInternal(rr["status"]);
			result.headers = ParseHeadersInternal(rr["headers"]);
			result.content = rr["content"];

			return result;
		}
	}
}