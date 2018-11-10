namespace CORE.Models
{
    public enum Status
    {
        Exception = 0,
        Success = 1,
        Warning = 2
    }

    public class Response
    {
        public Status Status { get; set; }
        public object Data { get; set; }
        public string Description { get; set; }

        public Response()
        {

        }

        public Response(object data)
        {
            Status = Status.Success;
            Data = data;            
        }
        
        public Response(Status status, string description)
        {
            Status = status;
            Description = description;
        }
    }
}
