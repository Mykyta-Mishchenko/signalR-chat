namespace chat_backend.Shared.Models
{
    public class OperationResult
    {
        public static OperationResult Success => new OperationResult() { Succeeded = true };
        public static OperationResult Failure => new OperationResult() { Failed = true };

        protected OperationResult() { }
        public bool Succeeded { get; protected set; }
        public bool Failed { get; protected set; }

        public string Message {  get; set; }
    }
}
