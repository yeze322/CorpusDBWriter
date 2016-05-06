namespace DataNormalizer.DataEntity
{
    public class ChatLog : IDataEntity
    {
        public int Id { get; set; }
        public int Dialogue { get; set; }
        public int Order { get; set; }
        public string TimeLabel { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public int IncidentId { get; set; }
        public string toJson() { return null; }
        public string generateJsonTemplate() { return null; }
    }
}