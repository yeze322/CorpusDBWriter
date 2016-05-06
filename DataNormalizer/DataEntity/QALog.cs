namespace DataNormalizer.DataEntity
{
    public class QALog : IDataEntity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int IncidentId { get; set; }
        public string toJson() { return null; }
        public string generateJsonTemplate() { return null; }
    }
}