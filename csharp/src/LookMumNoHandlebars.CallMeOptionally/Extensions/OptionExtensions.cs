// ReSharper disable MemberCanBePrivate.Global

using LookMumNoHandlebars.CallMeOptionally.Helpers;
namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionExtensions
{
    ///<summary>Returns value if some or provided fallback value is none</summary>
    public static T ValueOrDefault<T>(this Option<T> option, T fallbackValue) => option.Match(someVal => someVal, () => fallbackValue);
    
    /// <summary>Return the result of the operation that match a Some or None option</summary>
    /// <param name="option">Option on which to perform matching operation</param>
    /// <param name="someFunc">Function that executes on Some option</param>
    /// <param name="noneFunc">Function that executes on None option</param>
    /// <returns>Result of either Some or None matching function</returns>
    public static TResult Match<T, TResult>(this Option<T> option, Func<T, TResult> someFunc, Func<TResult> noneFunc) =>
        option.IsSome ? someFunc(option.Value) : noneFunc();

    /// <summary>Perform an action that matches a Some or None option</summary>
    /// <param name="option">Option on which to perform action</param>
    /// <param name="someFunc">Function that executes on Some option</param>
    /// <param name="noneFunc">Function that executes on None option</param>
    public static void Act<T>(this Option<T> option, Action<T> someFunc, Action noneFunc)
    {
        someFunc.ThrowIfNull();
        if (option.IsSome) someFunc(option.Value);
        else noneFunc();
    }
    
    /// <summary> Binds the option to a new option type</summary>
    /// <param name="option">Option on which to perform action</param>
    /// <param name="bindFunc">Function that that binds the original option type to the new option type</param>
    /// <returns>0ption bound to new type</returns>
    public static Option<TResult> Bind<T, TResult>(this Option<T> option, Func<T, Option<TResult>> bindFunc) {
        bindFunc.ThrowIfNull();
        return option.Match(bindFunc, Option.None<TResult>);
    }
    
    /// <summary>
    ///     Map an option type to another option type via an operation.
    /// </summary>
    /// <param name="option">Option on which to perform action</param>
    /// <param name="mapFunc">Function that maps the input option value to a new value (before </param>
    /// <returns></returns>
    public static Option<TReturn> Map<T, TReturn>(this Option<T> option, Func<T, TReturn> mapFunc) {
        mapFunc.ThrowIfNull();
        return option.Bind(someValue => Option.Some(mapFunc(someValue)));
    }
    
    /// <summary>
    ///     
    /// </summary>
    /// <param name="option"></param>
    /// <param name="mapFunc"></param>
    /// <returns>T</returns>
    public static Option<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, Option<TResult>> mapFunc)
    {
        mapFunc.ThrowIfNull();
        return option.Match(mapFunc, Option.None<TResult>);
    }
    
    /// <summary>Indicates whether the option value matches an input, using the defined `Equals` method of the type</summary>
    /// <param name="option">Input Option</param>
    /// <param name="value">Value to match against of the same type</param>
    /// <returns>True if Option is some and value returns true from 'Equals' check</returns>
    public static bool Contains<T>(this Option<T> option, T value)
    {
        return option.Match(
            someValue => someValue!.Equals(value),
            () => false
        );
    }
    
    public static Option<T> Filter<T>(this Option<T> option, Predicate<T> predicate)
    {
        return option.Match(
            someValue => predicate(someValue) ? Option.Some(someValue) : Option.None<T>(),
            Option.None<T>
        );
    }
    
    /// <summary>
    ///     Converts the option to a single list if some, an empty list if none.
    /// </summary>
    /// <param name="option"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> ToList<T>(this Option<T> option) =>
        option.Match(someVal => new List<T>() { someVal }, () => new List<T>());

    public static List<Option<T>> ToSingleList<T>(this Option<T> option) => new() { option };
}