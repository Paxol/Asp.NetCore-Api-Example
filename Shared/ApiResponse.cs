using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace DAMWeb.Shared
{
    public class ApiResponse
    {
        public bool Success { get; }
        public string ErrorDescription { get; }
        public object? Data { get; }

        [JsonConstructor]
        protected ApiResponse(bool success, string? errorDescription, object? data) 
        {
            Success = success;
            ErrorDescription = errorDescription ?? "Errore generico";
            Data = data;
        }

        public static ApiResponse Ok<T>(T? value) => new(true, null, value);

        public static ApiResponse Ok() => new(true, null, null);

        public static ApiResponse Problem<T>(string description, T? value) => new(false, description, value);

        public static ApiResponse Problem(string description) => new(false, description, null);

        public void Match<T>(Action<T?> ok, Action<string, T?> problem)
        {
            T? value = Data is T casted ? casted : default;

            if (Success) ok(value);
            else problem(ErrorDescription, value);
        }
    }
}
