namespace StockResearchPlatform.Commands
{
	/// <summary>
	/// Enum that holds inequality types Polygon accepts for inequality parameters
	/// </summary>
	public enum InequalityType
	{
		lte,  // Less than or equal to
		lt,   // Less than
		gte,  // Greater than or equal to
		gt,   // Greater than
		e     // Equal to (normal)
	}
	/// <summary>
	/// Class used to hold Polygon query parameters that can use inequalities (like less than, greater than, etc.)
	/// View the enum InequalityType to see the types
	/// </summary>
	public class PolygonParamInequality
	{
		public PolygonParamInequality(string parameterName, InequalityType type, string value) 
		{ 
			nameof = parameterName;
			value = value;
			switch (type)
			{
				case InequalityType.e:
					break;
				case InequalityType.lte:
					nameof += ".lte";
					break;
				case InequalityType.gt:
					nameof += ".gt";
					break;
				case InequalityType.lt:
					nameof += ".lt";
					break;
				case InequalityType.gte:
					nameof += ".gte";
					break;
			}
		}
		public string nameof { get; private set; }
		public string value { get; private set; }

		public bool IsNullOrEmpty()
		{
			return String.IsNullOrEmpty(value);
		}

	}
}
