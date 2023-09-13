namespace MlSuite.Domain
{
    public class EspelhoLog : EntityBase
    {
        public EspelhoLog(string caller, string message)
        {
            Caller = caller;
            Message = message;
        }

        public string Caller { get; set; }
        public string Message { get; set; }
    }
}
