

namespace Api.Users.Helpers
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public dynamic Data { get; set; }

        public Response()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
            Data = null;
        }
    }
}
