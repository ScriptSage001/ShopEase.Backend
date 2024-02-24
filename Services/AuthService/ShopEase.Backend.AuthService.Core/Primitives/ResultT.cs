namespace ShopEase.Backend.AuthService.Core.Primitives
{
    /// <summary>
    /// Genric Result Object
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Result<TValue> : Result
    {
        public readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The Value of the Failure Result cannot be accessed.");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
