// ReSharper disable MemberCanBePrivate.Global
using LookMumNoHandlebars.CallMeOptionally.Exceptions;

namespace LookMumNoHandlebars.CallMeOptionally;

public readonly struct Option<T> : IComparable<Option<T>>, IEquatable<Option<T>>
{
    private readonly T? _value;
    private readonly bool _isSome;
    
    /// <summary>Returns true if Value is 'Some' - has a definite value (is not null)</summary>
    public bool IsSome => _value is not null && _isSome;

    /// <summary>Returns true if Value is 'None' - has no definite value (is null)</summary>
    public bool IsNone => !IsSome;
    
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="UnhandledNoneException"></exception>
    public T Value
    {
        get
        {
            if (IsNone) throw new UnhandledNoneException(GetType().GetGenericArguments()[0]);
            return _value!;
        }
    }
    
    public Option(T? value)
    {
        _value = value;
        if (value is null)
        {
            _isSome = false;
        }
        _isSome = true;
    }

    /// <summary>Instantiates a 'None' option</summary>
    public Option()
    {
        _value = default;
        _isSome = false;
    }
    
    /// <summary>Factory Method to create Option from input value</summary>
    /// <param name="value">Input Value (may be null)</param>
    /// <returns>'Some' Option if value is not null, else 'None'</returns>
    public static Option<T> From(T? value) => new(value!);

    /// <summary>Factory Method to create Option from operation which may throw exception</summary>
    /// <param name="operation">Function that returns a Value that becomes the input for the option type</param>
    /// <returns>
    ///     The returned value from the operation as an Option.
    ///     If the operation throws an exception or returns null, the Option will be a 'None' Option
    ///     Else will be a 'Some' Option
    /// </returns>
    public static Option<T> SafeFrom(Func<T?> operation)
    {
        try
        {
            return From(operation());
        }
        catch
        {
            return new Option<T>();
        }
    }

    #region Interface Implementation
    public int CompareTo(Option<T> other)
    {
        return _isSome switch
        {
            true when !other._isSome => 1,
            false when other._isSome => -1,
            _ => Comparer<T>.Default.Compare(_value!, other._value!)
        };
    }
    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);
    public bool Equals(Option<T> other) => EqualityComparer<T?>.Default.Equals(_value, other._value) && _isSome == other._isSome;
    public bool Equals(T? other) => EqualityComparer<T?>.Default.Equals(_value, other);
    public override int GetHashCode() => HashCode.Combine(_value, _isSome);
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);
    #endregion
}

public static class Option {
    public static Option<T> Some<T>(T value) => new(value);
    public static Option<T> None<T>() => new();
}