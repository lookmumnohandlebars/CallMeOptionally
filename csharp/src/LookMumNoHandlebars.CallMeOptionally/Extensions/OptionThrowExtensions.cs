using LookMumNoHandlebars.CallMeOptionally.Exceptions;
using Throw;

namespace LookMumNoHandlebars.CallMeOptionally.Extensions;

public static class OptionThrowExtensions
{
    public static ref readonly Validatable<Option<T>> IfNone<T>(this in Validatable<Option<T>> validatableOption)
    {
        if (!validatableOption.Value.IsSome) 
            throw new ArgumentException("Option should not be None", new UnhandledNoneException(validatableOption.GetType()));
        return ref validatableOption;
    }
    
    public static ref readonly Validatable<Option<T>> IfSome<T>(this in Validatable<Option<T>> validatableOption)
    {
        if (validatableOption.Value.IsSome) 
            throw new ArgumentException("Option should not be Some", new UnhandledNoneException(validatableOption.GetType()));
        return ref validatableOption;
    }
    
    public static ref readonly Validatable<Option<T>> IfNoneOrSomeFailsCondition<T>(this in Validatable<Option<T>> validatableOption, Predicate<Validatable<T>> condition) where T : notnull
    {
        var optionToValidate = validatableOption;
        if (!optionToValidate.Value.IsSome)
        {
            throw new ArgumentException("Option should not be None",
                new UnhandledNoneException(validatableOption.GetType()));
        }
        if (!optionToValidate.Value.Filter(value => condition(new Validatable<T>(value, optionToValidate.ParamName, new ExceptionCustomizations()))).IsSome)
        {
            throw new ArgumentException("Option should not be fail", new UnhandledNoneException(validatableOption.GetType()));
        }
        return ref validatableOption;
    }
}