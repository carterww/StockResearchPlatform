﻿using System.Globalization;
using System.Text.Json;

namespace StockResearchPlatform.Services.Polygon
{
	public class PolygonBaseService
	{
		private readonly HttpService _httpService;

		public PolygonBaseService(HttpService httpservice)
		{
			_httpService = httpservice;
		}

		/// <summary>
		/// Calls an enpoint and parses the HTTP response's content into object specified by generic type
		/// </summary>
		/// <typeparam name="Jto">Type to be instantiated from HTTP res's JSON content</typeparam>
		/// <param name="endpointUrl">Polygon URL to call</param>
		/// <returns>A Jto(JSON to object) object</returns>
		public async Task<Jto> GetJto<Jto>(string endpointUrl)
		{
			using (var client = new HttpClient(_httpService.Handler, false))
			{ 
				try
				{
					HttpResponseMessage? res = null;
					using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpointUrl))
					{
						res = client.SendAsync(requestMessage).Result;
					}
					if (res != null)
					{
						res.EnsureSuccessStatusCode();
						string resBody = await res.Content.ReadAsStringAsync();

						return JsonSerializer.Deserialize<Jto>(resBody);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			return default;
			}
		}

		public DateTime ParsePolygonDate(string polygonDate)
		{
			return DateTime.ParseExact(polygonDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
		}
	}
}
