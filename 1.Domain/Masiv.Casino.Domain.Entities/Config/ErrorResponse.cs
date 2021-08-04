    using System;

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

        public class BaseError : Exception
        {
            public string ResultCode { get; set; }
            public string ResultMsg { get; set; }
            public int StatusCode { get; set; }

            public BaseError(string resultCode, string resultMsg)
            {
                ResultCode = resultCode;
                ResultMsg = resultMsg;
            }
        }

        public class BadRequest : BaseError
        {

            public BadRequest(string resultCode, string resultMsg) : base(resultCode, resultMsg)
            {
                this.StatusCode = 400;
            }
        }
    }