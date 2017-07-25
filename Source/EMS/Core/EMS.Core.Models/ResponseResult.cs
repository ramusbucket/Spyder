namespace EMS.Core.Models
{
    public class ResultResponse<T> : EmptyResponse
    {
        public T Result { get; set; }
    }

    public class EmptyResponse
    {
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
