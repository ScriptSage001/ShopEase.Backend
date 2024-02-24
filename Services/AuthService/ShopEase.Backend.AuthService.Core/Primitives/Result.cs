namespace ShopEase.Backend.AuthService.Core.Primitives
{
    /// <summary>
    /// Result Object
    /// </summary>
    public class Result
    {
        #region Properties

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        #endregion

        #region Constructor

        protected internal Result(bool isSuccess, Error error) 
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            {
                throw new InvalidOperationException("Invalid Result Scenario.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        #endregion

        #region Methods

        public static Result Success() => new(true, Error.None);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        public static Result Create(bool condition) => condition ?
                                                        Success() : Failure(Error.ConditionNotMet);

        public static Result<TValue> Create<TValue>(TValue? value) => value is not null ?
                                                                        Success<TValue>(value) : Failure<TValue>(Error.NullValue);

        #endregion
    }
}
