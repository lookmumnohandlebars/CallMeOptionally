// ReSharper disable MemberCanBePrivate.Global
using LookMumNoHandlebars.CallMeOptionally.Exceptions;

namespace LookMumNoHandlebars.CallMeOptionally;

public readonly struct Option<T> : IComparable<Option<T>>, IEquatable<Option<T>>
{
    private readonly T? _value;
    private readonly bool _isSome;
    
    public bool IsSome => _value is not null && _isSome;

    public T Value
    {
        get
        {
            if (!IsSome) throw new UnhandledNoneException(GetType().GetGenericArguments()[0]);
            return _value!;
        }
    }
    
    public Option(T value)
    {
        _value = value;
        if (value is null)
        {
            _isSome = false;
        }
        _isSome = true;
    }

    public Option()
    {
        _value = default;
        _isSome = false;
    }
    
    public static Option<T> From(T? value) => new(value!);

    public static Option<T> SafeFrom(Func<T?> valueFunc)
    {
        try
        {
            return new Option<T>(valueFunc()!);
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
            _ => Comparer<T>.Default.Compare(_value, other._value)
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