namespace StockResearchPlatform.Models
{
    // This class holds all properties returned from Aletheia API v2 /StockData endpoint
    public class StockInformation
    {
        public string? Symbol { get; set; }
        public long? FullTimeEmployees { get; set; }
		public string? Sector { get; set; }
		public string? Industry { get; set; }
		public double? Open { get; set; }
		public long? AverageVolume90Day { get; set; }
		public string? Exchange { get; set; }
		public double? DayHigh { get; set; }
		public string? ShortName { get; set; }
		public string? LongName { get; set; }
		public double? Change { get; set; }
		public double? PreviousClose { get; set; }
		public double? Price { get; set; }
		public string? Currency { get; set; }
		public long? Volume { get; set; }
		public double? MarketCap { get; set; }
		public double? ChangePercent { get; set; }
		public double? DayLow { get; set; }
		public long? AverageVolume10Day { get; set; }
		public double? YearLow { get; set; }
		public double? YearHigh { get; set; }
		public double? Beta { get; set; }
		public double? PriceEarningsRatio { get; set; }
		public double? EarningsPerShare { get; set; }
		public DateTime? EarningsDate { get; set; }
		public double? ForwardDividend { get; set; }
		public double? ForwardDividendYield { get; set; }
		public DateTime? ExDividendDate { get; set; }
		public double? YearTargetEstimate { get; set; }
		public DateTime? LastFiscalYearEnd { get; set; }
		public DateTime? LastFiscalQuarterEnd { get; set; }

		public double? ProfitMargin { get; set; }
		public double? OperatingMargin { get; set; }
		public double? ReturnOnAssets { get; set; }
		public double? ReturnOnEquity { get; set; }
		public double? Revenue { get; set; }
		public double? RevenuePerShare { get; set; }
		public double? QuarterlyRevenueGrowth { get; set; }
		public double? GrossProfit { get; set; }
		public double? EDBITDA { get; set; }
		public double? NetIncomeAvailableToCommon { get; set; }
		public double? QuarterlyEarningsGrowth { get; set; }
		public double? Cash { get; set; }
		public double? CashPerShare { get; set; }
		public double? Debt { get; set; }
		public double? DebtToEquityRatio { get; set; }
		public double? CurrentRatio { get; set; }
		public double? BookValuePerShare { get; set; }
		public double? OperatingCashFlow { get; set; }
		public double? LeveredFreeCashFlow { get; set; }
		public double? YearChangePercent { get; set; }
		public double? SP500YearChangePercent { get; set; }
		public double? MovingAverage50Day { get; set; }
		public double? MovingAverage200Day { get; set; }
		public double? SharesOutstanding { get; set; }
		public double? Float { get; set; }
		public double? PercentHeldByInsiders { get; set; }
		public double? PercentHeldByInstitutions { get; set; }
		public double? SharesShort { get; set; }
		public double? ShortRatio { get; set; }
		public double? ShortPercentOfFloat { get; set; }
		public double? ShortPercentOfSharesOutstanding { get; set; }
		public double? ForwardAnnualDividend { get; set; }
		public double? ForwardAnnualDividendYield { get; set; }
		public double? TrailingAnnualDividend { get; set; }
		public double? TrailingAnnualDividendYield { get; set; }
		public double? FiveYearAverageDividendYield { get; set; }
		public double? DividendPayoutRatio { get; set; }
		public DateTime? DividendDate { get; set; }
		public string? LastSplitFactor { get; set; }
		public DateTime? LastSplitDate { get; set; }
	}
}
