using System.Reflection;

namespace LookMumNoHandlebars.CallMeOptionally.Exceptions;

public class UnhandledNoneException : NullReferenceException
{
    public UnhandledNoneException(MemberInfo type) : base($"Unhandled None value for Option type {type.Name}")
    {
        
    }
}