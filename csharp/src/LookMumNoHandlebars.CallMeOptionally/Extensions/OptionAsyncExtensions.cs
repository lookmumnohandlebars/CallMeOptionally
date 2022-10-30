// ReSharper disable MemberCanBePrivate.Global
namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionAsyncExtensions
{
    public static async Task<TResult> MatchAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> someFunc, Func<Task<TResult>> noneFunc) =>
        option.IsSome ? await someFunc(option.Value).ConfigureAwait(false) : await noneFunc().ConfigureAwait(false);

    public static async Task ActAsync<T>(this Option<T> option, Func<T, Task> someFunc, Func<Task> noneFunc)
    {
        if (option.IsSome) await someFunc(option.Value).ConfigureAwait(false);
        else await noneFunc().ConfigureAwait(false);
    }
    public static async Task<Option<TResult>> BindAsync<T, TResult>(this Option<T> option, Func<T, Task<Option<TResult>>> bindFunc) {
        return await option.MatchAsync(bindFunc, () => Task.FromResult(Option.None<TResult>()));
    }
    
    public static Task<Option<TReturn>> MapAsync<T, TReturn>(this Option<T> option, Func<T, Task<TReturn>> mapFunc) {
        return option.BindAsync(async someValue => Option.Some(await mapFunc(someValue).ConfigureAwait(false)));
    }
    
    public static Task<Option<TResult>> FlatMapAsync<T, TResult>(this Option<T> option, Func<T, Task<Option<TResult>>> mapFunc) {
        return option.MatchAsync(mapFunc, () => Task.FromResult(Option.None<TResult>()));
    }
    
    public static Task<Option<T>> FilterAsync<T>(this Option<T> option, Func<T, Task<bool>> predicate)
    {
        return option.MatchAsync(
            async someValue => await predicate(someValue).ConfigureAwait(false) ? Option.Some(someValue) : Option.None<T>(),
            () => Task.FromResult(Option.None<T>())
        );
    }
}