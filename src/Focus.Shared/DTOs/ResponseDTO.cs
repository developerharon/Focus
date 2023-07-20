using Focus.Shared.Enums;

namespace Focus.Shared.DTOs
{
    public class ResponseDTO
    {
        public ResponseType ResponseType { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ResponseDTO Create(ResponseType type, string message = "")
        {
            return new ResponseDTO { ResponseType = type, Message = message };
        }
    }

    public class ResponseDTO<T> : ResponseDTO
    {
        public T? Data { get; set; }

        public static ResponseDTO<T> Create(ResponseType type, T? data, string message = "")
        {
            return new ResponseDTO<T> { ResponseType = type, Data = data, Message = message };
        }
    }
}