namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionAsyncEnumerableExtensions
{
    public static IAsyncEnumerable<Option<T>> ToOptionalAsyncEnumerable<T>(this IAsyncEnumerable<T> items) =>
        items.Select(Option<T>.From);
    
    public static ValueTask<bool> ContainsSomeAsync<T>(this IAsyncEnumerable<Option<T>> options) =>
        options.AnyAsync(option => option.IsSome);
    public static ValueTask<bool> ContainsNoneAsync<T>(this IAsyncEnumerable<Option<T>> options) =>
        options.AnyAsync(option => !option.IsSome);
    
    public static IAsyncEnumerable<Option<T>> WhereSomeAsync<T>(this IAsyncEnumerable<Option<T>> options) =>
        options.Where(option => option.IsSome);
    public static IAsyncEnumerable<Option<T>> WhereNoneAsync<T>(this IAsyncEnumerable<Option<T>> options) =>
        options.Where(option => !option.IsSome);

    public static IAsyncEnumerable<Option<T>> FilterAsync<T>(this IAsyncEnumerable<Option<T>> options, Predicate<T> filter) =>
        options.Select(option => option.Filter(filter));

    public static IAsyncEnumerable<Option<TValue>> SelectSomeAsync<TSource, TValue>(
        this IAsyncEnumerable<Option<TSource>> options,
        Func<TSource, TValue> selector
    ) => options.Select(option => option.Map(selector));
    
    public static IAsyncEnumerable<Option<TValue>> SelectAsync<TSource, TValue>(
        this IAsyncEnumerable<Option<TSource>> options,
        Func<TSource, TValue> someSelector,
        Func<TValue> noneSelector
    ) => options.Select(
        option => option.Match(
            value => Option.Some(someSelector(value)), 
            () => Option<TValue>.From(noneSelector())
        )
    );
}