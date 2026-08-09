namespace Domain.Shared;

public class Result<T>:Result
{
    private readonly T _value;

    protected Result(T value,bool isSuccess, Error error)
        :base(isSuccess, error)=>_value=value;

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException(
            "Cannot access the value of a failed result.");

    public static Result<T> Success(T value) =>
        new(value,true, Error.None);
    public new static Result<T> Failure(Error error) =>
        new(default!, false, error);

    public static implicit operator Result<T>(T value) =>
        Success(value);

    public static implicit operator Result<T>(Error error)
        => Failure(error);
}
