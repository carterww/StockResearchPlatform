namespace StockResearchPlatform.Models.PolygonModels
{
	// StockFinancialsVX myDeserializedClass = JsonConvert.DeserializeObject<StockFinancialsVX>(myJsonResponse);
	public class Assets
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public long value { get; set; }
	}

	public class BalanceSheet
	{
		public Assets assets { get; set; }
		public CurrentAssets current_assets { get; set; }
		public CurrentLiabilities current_liabilities { get; set; }
		public Equity equity { get; set; }
		public EquityAttributableToNoncontrollingdoubleerest equity_attributable_to_noncontrolling_doubleerest { get; set; }
		public EquityAttributableToParent equity_attributable_to_parent { get; set; }
		public Liabilities liabilities { get; set; }
		public LiabilitiesAndEquity liabilities_and_equity { get; set; }
		public NoncurrentAssets noncurrent_assets { get; set; }
		public NoncurrentLiabilities noncurrent_liabilities { get; set; }
	}

	public class BasicEarningsPerShare
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class BenefitsCostsExpenses
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class CashFlowStatement
	{
		public ExchangeGainsLosses exchange_gains_losses { get; set; }
		public NetCashFlow net_cash_flow { get; set; }
		public NetCashFlowContinuing net_cash_flow_continuing { get; set; }
		public NetCashFlowFromFinancingActivities net_cash_flow_from_financing_activities { get; set; }
		public NetCashFlowFromFinancingActivitiesContinuing net_cash_flow_from_financing_activities_continuing { get; set; }
		public NetCashFlowFromInvestingActivities net_cash_flow_from_investing_activities { get; set; }
		public NetCashFlowFromInvestingActivitiesContinuing net_cash_flow_from_investing_activities_continuing { get; set; }
		public NetCashFlowFromOperatingActivities net_cash_flow_from_operating_activities { get; set; }
		public NetCashFlowFromOperatingActivitiesContinuing net_cash_flow_from_operating_activities_continuing { get; set; }
	}

	public class ComprehensiveIncome
	{
		public ComprehensiveIncomeLoss comprehensive_income_loss { get; set; }
		public ComprehensiveIncomeLossAttributableToNoncontrollingdoubleerest comprehensive_income_loss_attributable_to_noncontrolling_doubleerest { get; set; }
		public ComprehensiveIncomeLossAttributableToParent comprehensive_income_loss_attributable_to_parent { get; set; }
		public OtherComprehensiveIncomeLoss other_comprehensive_income_loss { get; set; }
		public OtherComprehensiveIncomeLossAttributableToParent other_comprehensive_income_loss_attributable_to_parent { get; set; }
	}

	public class ComprehensiveIncomeLoss
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class ComprehensiveIncomeLossAttributableToNoncontrollingdoubleerest
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class ComprehensiveIncomeLossAttributableToParent
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class CostOfRevenue
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class CostsAndExpenses
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class CurrentAssets
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class CurrentLiabilities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class DilutedEarningsPerShare
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class Equity
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class EquityAttributableToNoncontrollingdoubleerest
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class EquityAttributableToParent
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class ExchangeGainsLosses
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class Financials
	{
		public BalanceSheet balance_sheet { get; set; }
		public CashFlowStatement cash_flow_statement { get; set; }
		public ComprehensiveIncome comprehensive_income { get; set; }
		public IncomeStatement income_statement { get; set; }
	}

	public class GrossProfit
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class IncomeLossFromContinuingOperationsAfterTax
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class IncomeLossFromContinuingOperationsBeforeTax
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class IncomeStatement
	{
		public BasicEarningsPerShare basic_earnings_per_share { get; set; }
		public BenefitsCostsExpenses benefits_costs_expenses { get; set; }
		public CostOfRevenue cost_of_revenue { get; set; }
		public CostsAndExpenses costs_and_expenses { get; set; }
		public DilutedEarningsPerShare diluted_earnings_per_share { get; set; }
		public GrossProfit gross_profit { get; set; }
		public IncomeLossFromContinuingOperationsAfterTax income_loss_from_continuing_operations_after_tax { get; set; }
		public IncomeLossFromContinuingOperationsBeforeTax income_loss_from_continuing_operations_before_tax { get; set; }
		public IncomeTaxExpenseBenefit income_tax_expense_benefit { get; set; }
		public doubleerestExpenseOperating doubleerest_expense_operating { get; set; }
		public NetIncomeLoss net_income_loss { get; set; }
		public NetIncomeLossAttributableToNoncontrollingdoubleerest net_income_loss_attributable_to_noncontrolling_doubleerest { get; set; }
		public NetIncomeLossAttributableToParent net_income_loss_attributable_to_parent { get; set; }
		public NetIncomeLossAvailableToCommonStockholdersBasic net_income_loss_available_to_common_stockholders_basic { get; set; }
		public OperatingExpenses operating_expenses { get; set; }
		public OperatingIncomeLoss operating_income_loss { get; set; }
		public ParticipatingSecuritiesDistributedAndUndistributedEarningsLossBasic participating_securities_distributed_and_undistributed_earnings_loss_basic { get; set; }
		public PreferredStockDividendsAndOtherAdjustments preferred_stock_dividends_and_other_adjustments { get; set; }
		public Revenues revenues { get; set; }
	}

	public class IncomeTaxExpenseBenefit
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class doubleerestExpenseOperating
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class Liabilities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class LiabilitiesAndEquity
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public long value { get; set; }
	}

	public class NetCashFlow
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowContinuing
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromFinancingActivities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromFinancingActivitiesContinuing
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromInvestingActivities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromInvestingActivitiesContinuing
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromOperatingActivities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetCashFlowFromOperatingActivitiesContinuing
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetIncomeLoss
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetIncomeLossAttributableToNoncontrollingdoubleerest
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetIncomeLossAttributableToParent
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NetIncomeLossAvailableToCommonStockholdersBasic
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NoncurrentAssets
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class NoncurrentLiabilities
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class OperatingExpenses
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class OperatingIncomeLoss
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class OtherComprehensiveIncomeLoss
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class OtherComprehensiveIncomeLossAttributableToParent
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class ParticipatingSecuritiesDistributedAndUndistributedEarningsLossBasic
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class PreferredStockDividendsAndOtherAdjustments
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class Result
	{
		public string cik { get; set; }
		public string company_name { get; set; }
		public string end_date { get; set; }
		public string filing_date { get; set; }
		public Financials financials { get; set; }
		public string fiscal_period { get; set; }
		public string fiscal_year { get; set; }
		public string source_filing_file_url { get; set; }
		public string source_filing_url { get; set; }
		public string start_date { get; set; }
	}

	public class Revenues
	{
		public string label { get; set; }
		public double order { get; set; }
		public string unit { get; set; }
		public double value { get; set; }
	}

	public class StockFinancialsVXJto
	{
		public double count { get; set; }
		public string next_url { get; set; }
		public string request_id { get; set; }
		public List<Result> results { get; set; }
		public string status { get; set; }
	}


}
