namespace TalabatAPIs.Errors
{
    public class ApiVallidationErrorResponse : ApiRespone
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiVallidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }
    }
}
