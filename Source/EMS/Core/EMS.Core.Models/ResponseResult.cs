namespace EMS.Core.Models
{
    public class ResultResponse<T> : EmptyResponse
    {
        public ResultResponse()
        {

        }

        public ResultResponse(T result, string message = "", bool isSuccessful = true)
        {
            this.Result = result;
            this.Message = message;
            this.IsSuccessful = isSuccessful;
        }

        public T Result { get; set; }
    }

    public class EmptyResponse
    {
        public EmptyResponse()
        {
        }

        public EmptyResponse(string message, bool isSuccessful)
        {
            this.Message = message;
            this.IsSuccessful = isSuccessful;
        }

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
