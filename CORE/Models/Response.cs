namespace CORE.Models
{
    public enum Status
    {
        Error = 0,
        Success = 1
    }

    public class Response
    {
        public Status Status { get; set; }
        public object Data { get; set; }
        public string Error { get; set; }

        public Response()
        {

        }

        public Response(object data)
        {
            Status = Status.Success;
            Data = data;            
        }

        public Response(string error)
        {
            Status = Status.Error;
            Error = error;
        }
    }
}
