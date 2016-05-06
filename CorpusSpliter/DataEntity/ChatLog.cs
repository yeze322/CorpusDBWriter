namespace CorpusSpliter.DataEntiyy
{
    public class ChatLog
    {
        public int Id { get; set; }
        public int Dialogue { get; set; }
        public int Order { get; set; }
        public string TimeLabel { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public int IncidentId { get; set; }
    }
}