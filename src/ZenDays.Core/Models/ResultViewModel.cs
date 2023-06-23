namespace ZenDays.Core.Models
{
    public class ResultViewModel
    {
        public dynamic? Data { get; private set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; private set; }
        public ResultViewModel(dynamic? data, int statusCode, bool success, string message = "")
        {
            Data = data;
            StatusCode = statusCode;
            Success = success;
            Message = message;
        }
    }
}
