namespace API.Abstraction.Helpers
{
    public class Result<T>
    {
        public bool Error { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }
        public static Result<T> Success(T value) => new Result<T> { Error = false, Value = value };
        public static Result<T> Failure(string message) => new Result<T> { Error = true, ErrorMessage = message };
    }
}