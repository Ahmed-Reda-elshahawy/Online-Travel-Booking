using OnlineTravelBooking.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.ViewModels
{
    public class ResultViewModel<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public string Message { get; private set; }
        public int StatusCode { get; private set; }

        public static ResultViewModel<T> Ok(T data, string message="messages retrieved successfully",int statusCode=201) => new() { Success = true, Data = data, Message = message, StatusCode = statusCode };
        public static ResultViewModel<T> Fail(string error) => new() { Success = false, ErrorMessage = error };
        public static ResultViewModel<T> Fail(Dictionary<string, string[]> errors) => new() { Success = false, Errors = errors };
    }
}
