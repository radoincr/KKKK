using System.Diagnostics.CodeAnalysis;

namespace INV.Domain.Shared;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None) throw new InvalidCastException();

        if (!isSuccess && error == Error.None) throw new InvalidCastException();

        IsSuccess = isSuccess;
        Error = error;
    }

    protected Result(bool isSuccess, List<Error> error)
    {
        if (isSuccess && error.Any()) throw new InvalidCastException();

        if (!isSuccess && error.Any() == false) throw new InvalidCastException();

        IsSuccess = isSuccess;
        Errors = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    public List<Error> Errors { get; }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result Failure(List<Error> errors)
    {
        return new Result(false, errors);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    public static Result<T> Failure<T>(Error error)
    {
        return new Result<T>(default!, false, error);
    }

    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }

    public static Result<T> Create<T>(T? value)
    {
        return value is not null ? Success(value) : Failure<T>(Error.NullValue);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed");

    public static implicit operator Result<T>(Error error)
    {
        return Failure<T>(error);
    }

    public static implicit operator Result<T>(T? value)
    {
        return Create(value);
    }
}