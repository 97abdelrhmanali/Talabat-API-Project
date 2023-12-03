namespace TalabatAPIs.Errors
{
    public class ApiServerErrorResponse : ApiRespone
    {
        public string? Details { get; set; }
        public ApiServerErrorResponse(int status , string? message = null , string? details = null)
            : base(status, message)
        {
            Details= details;
        }
    }
}
