namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionEnumerableExtensions
{
    public static IEnumerable<Option<T>> ToOptionalEnumerable<T>(this IEnumerable<T> items) =>
        items.Select(Option<T>.From);
    
    public static bool ContainsSome<T>(this IEnumerable<Option<T>> options) =>
        options.Any(option => option.IsSome);
    public static bool ContainsNone<T>(this IEnumerable<Option<T>> options) =>
        options.Any(option => !option.IsSome);
    
    public static IEnumerable<Option<T>> WhereSome<T>(this IEnumerable<Option<T>> options) =>
        options.Where(option => option.IsSome);
    public static IEnumerable<Option<T>> WhereNone<T>(this IEnumerable<Option<T>> options) =>
        options.Where(option => !option.IsSome);

    public static IEnumerable<Option<T>> Filter<T>(this IEnumerable<Option<T>> options, Predicate<T> filter) =>
        options.Select(option => option.Filter(filter));

    public static IEnumerable<Option<TValue>> SelectSome<TSource, TValue>(
        this IEnumerable<Option<TSource>> options,
        Func<TSource, TValue> selector
    ) => options.Select(option => option.Map(selector));
    
    public static IEnumerable<Option<TValue>> Select<TSource, TValue>(
        this IEnumerable<Option<TSource>> options,
        Func<TSource, TValue> someSelector,
        Func<TValue> noneSelector
    ) => options.Select(
        option => option.Match(
            value => Option.Some(someSelector(value)), 
            () => Option<TValue>.From(noneSelector())
        )
    );
}