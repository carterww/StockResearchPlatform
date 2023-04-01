namespace StockResearchPlatform.Commands
{
	public enum InequalityType
	{
		lte,  // Less than or equal to
		lt,   // Less than
		gte,  // Greater than or equal to
		gt,   // Greater than
		e     // Equal to (normal)
	}
	public class PolygonParamInequality
	{
		public PolygonParamInequality(string parameterName, InequalityType type) 
		{ 
			nameof = parameterName;
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

	}
}
