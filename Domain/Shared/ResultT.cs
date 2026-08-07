using Domain.Abstractions;

namespace Domain.Shared;

public class Result<T>:Result
{
    protected Result(T value,bool isSuccess, Error? error)
        :base(isSuccess, error)=>Value=value;
    
    public T Value { get; }
    
    public static Result<T> Success(T value) => 
        new(value,true, Error.None);
    public new static Result<T> Failure(Error error) =>
        new(default!, false, error);
    
    public static implicit operator Result<T>(T value) => 
        Success(value);
    
    public static implicit operator Result<T>(Error error) 
        => Failure(error);
}