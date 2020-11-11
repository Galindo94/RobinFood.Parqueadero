namespace Common.Utils.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseDto(bool success, string message, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        public ResponseDto()
        {
        }
    }
}