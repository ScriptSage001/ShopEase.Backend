namespace ShopEase.Backend.AuthService.Core.Primitives
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public Error(string code, string message, object? data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
