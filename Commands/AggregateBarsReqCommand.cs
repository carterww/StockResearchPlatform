﻿using System.Text;
using System;

namespace StockResearchPlatform.Commands
{
	public enum Timespan
	{
		minute,
		hour,
		day,
		week,
		month,
		quarter,
		year
	}
	public class AggregateBarsReqCommand : IReqCommand
	{
		public string stocksTicker { get; set; }
		public int multiplier { get; set; } = 1;
		public Timespan timespan { get; set; } = Timespan.day;
		public DateTime from { get; set; }
		public DateTime to { get; set; }
		public bool adjusted { get; set; } = true;
		public string sort { get; set; } = "desc";
		public int limit { get; set; } = 5000;

		public string BuildQueryParams(string baseUrl, string endpoint, string apiKey)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(baseUrl);
			sb.Append(endpoint);
			sb.Append($"?apiKey={apiKey}");

			#region AddParams
			endpoint = string.Format(endpoint, stocksTicker.ToUpper(), multiplier, timespan.ToString(), string.Format("{0:yyyy-MM-dd}", from), string.Format("{0:yyyy-MM-dd}", to));

			sb.Append($"&adjusted={adjusted}");
			sb.Append($"&sort={sort}");
			sb.Append($"&limit={sort}");

			#endregion

			return sb.ToString();
		}
	}
}
