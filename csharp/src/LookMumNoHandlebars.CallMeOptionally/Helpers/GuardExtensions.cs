namespace LookMumNoHandlebars.CallMeOptionally.Helpers;

internal static class GuardExtensions
{
    public static void ThrowIfNull<T>(this T? value)
    {
        if (value is null) throw new NullReferenceException("Explicitly disallowed use of null value");
    }
}