using MusicLyricApp.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace MusicLyricApp.Api.Music
{
	public abstract class BaseNativeApi(Func<string> cookieFunc)
	{
		public const string UserAgent =
			"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";

		public const string DefaultCookie =
			"os=pc;osver=Microsoft-Windows-10-Professional-build-16299.125-64bit;appver=2.0.3.131777;channel=netease;__remember_me=true";

		private readonly Func<string> _cookieFunc = cookieFunc;

		protected abstract string HttpRefer();

		private readonly HttpClient httpClient = new();

		/// <summary>
		/// 
		/// </summary>
		/// <content name="url">链接</content>
		/// <content name="paramDict">参数</content>
		/// <exception cref="WebException"></exception>
		/// <returns></returns>
		protected string SendPost(string url, Dictionary<string, string> paramDict)
		{
			var cookie = _cookieFunc.Invoke();
			if (string.IsNullOrWhiteSpace(cookie))
			{
				cookie = DefaultCookie;
			}

			var req = new HttpRequestMessage
			{
				RequestUri = new Uri(url),
				Method = HttpMethod.Post,
				Headers =
				{
					{ HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded" },
					{ HttpRequestHeader.UserAgent.ToString(), UserAgent },
					{ HttpRequestHeader.Cookie.ToString(), cookie },
					{ HttpRequestHeader.Referer.ToString(), HttpRefer() }
				},
				Content = new FormUrlEncodedContent(paramDict),
			};

			var response = httpClient.SendAsync(req).Result;
			var contents = response.Content.ReadAsStringAsync().Result;

			return contents;
		}

		protected string SendJsonPost(string url, Dictionary<string, object> paramDict)
		{
			var cookie = _cookieFunc.Invoke();
			if (string.IsNullOrWhiteSpace(cookie))
			{
				cookie = DefaultCookie;
			}

			var req = new HttpRequestMessage
			{
				RequestUri = new Uri(url),
				Method = HttpMethod.Post,
				Headers =
				{
					{ HttpRequestHeader.ContentType.ToString(), "application/json" },
					{ HttpRequestHeader.UserAgent.ToString(), UserAgent },
					{ HttpRequestHeader.Cookie.ToString(), cookie },
					{ HttpRequestHeader.Referer.ToString(), HttpRefer() }
				},
				Content = new StringContent(paramDict.ToJson()),
			};

			var response = httpClient.SendAsync(req).Result;
			var contents = response.Content.ReadAsStringAsync().Result;

			return contents;
		}
	}
}
