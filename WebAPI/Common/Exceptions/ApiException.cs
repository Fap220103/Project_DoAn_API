namespace WebAPI.Common.Exceptions
{
    public class ApiException : Exception
    {
        public int Code { get; }
        public override string Message { get; }

        public ApiException(int statusCode, string statusMessage)
        {
            Code = statusCode;
            Message = statusMessage;
        }
    }
}
