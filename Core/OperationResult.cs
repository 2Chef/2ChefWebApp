namespace Core
{
    public class OperationResult
    {
        public bool IsSuccess { get; }

        public string ErrorMessage { get; }

        public OperationResult(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public OperationResult(bool isSuccess) : this(isSuccess, "") { }

        public OperationResult() : this(true, "") { }
    }
}
