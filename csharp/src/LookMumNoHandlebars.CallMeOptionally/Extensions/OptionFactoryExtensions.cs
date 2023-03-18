namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionFactory
{
    /// <summary>Convert value to option</summary>
    /// <param name="value">Value (nullable) of any type</param>
    /// <returns>Value as option</returns>
    public static Option<T> ToOption<T>(this T? value) => Option<T>.From(value);

    public static Option<TNew> ToOption<T, TNew>(this T? value, Func<T, TNew> mapFunc)
        => value.ToOption().Map(mapFunc);

    public static IEnumerable<Option<T>> ToOptions<T>(this IEnumerable<T?> valueList)
        => valueList.Select(value => value.ToOption());
    
    public static IEnumerable<Option<TNew>> ToOptions<T, TNew>(this IEnumerable<T?> valueList, Func<T, TNew> mapFunc)
        => valueList.Select(value => value.ToOption(mapFunc));

    public static Option<T> ToSafeOption<T>(Func<T> producerFunc)
    {
        try
        {
            return Option<T>.From(producerFunc()!);
        }
        catch (Exception)
        {
            return Option.None<T>();
        }
    }
}