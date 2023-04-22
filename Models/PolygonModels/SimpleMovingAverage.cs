namespace StockResearchPlatform.Models.PolygonModels
{
    public class SMAResults
    {
        public Underlying underlying { get; set; }
        public List<Value> values { get; set; }
    }

    public class SimpleMovingAverageV1Jto
    {
        public SMAResults results { get; set; }
        public string status { get; set; }
        public string request_id { get; set; }
        public string next_url { get; set; }
    }

    public class Underlying
    {
        public string url { get; set; }
    }

    public class Value
    {
        public long timestamp { get; set; }
        public double value { get; set; }
    }
}
