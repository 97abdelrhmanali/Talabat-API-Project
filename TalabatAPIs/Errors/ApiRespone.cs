namespace TalabatAPIs.Errors
{
    public class ApiRespone
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiRespone(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);  
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authrized, you are not authrized",
                404 => "Resourse was not found",
                500 => "Errors are the path to the dark side. Errors leads to anger. anger leads to hate. Hate leads to career change",
                _ => null,
            };
        }
    }
}
