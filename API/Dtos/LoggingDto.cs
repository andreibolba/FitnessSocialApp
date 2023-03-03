namespace API.Dtos
{
    public class LoggingDto
    {
        public int LogId { get; set; }
        public string LogType { get; set; }

        public string LogMessage { get; set; }

        public string PersonUsername { get; set; }

        public DateTime DateOfLog { get; set; }
    }
}