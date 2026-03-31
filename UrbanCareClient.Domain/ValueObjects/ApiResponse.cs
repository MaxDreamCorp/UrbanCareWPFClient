using System.Net;
using UrbanCareClient.Domain.DTOs;

namespace UrbanCareClient.Domain.ValueObjects
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public List<ErrorDTO>? Errors { get; private set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ApiResponse<T> Success(T? data) => new()
        {
            IsSuccess = true,
            Data = data,
            StatusCode = HttpStatusCode.OK
        };

        public static ApiResponse<T> GetError(List<ErrorDTO> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            new()
            {
                IsSuccess = false,
                Errors = errors,
                StatusCode = statusCode
            };
    }
}
