namespace Masiv.Casino.Domain.Entities.Config
{
    public class ErrorResponse
    {
        public string ResultCode { get; set; }
        public string ResultMsg { get; set; }

        public ErrorResponse()
        {
        }

        public ErrorResponse(string resultCode, string resultMsg)
        {
            this.ResultCode = resultCode;
            this.ResultMsg = resultMsg;
        }
    }
}