using FluentAssertions;
using LookMumNoHandlebars.CallMeOptionally.Exceptions;
using LookMumNoHandlebars.CallMeOptionally.Tests.Helpers;

namespace LookMumNoHandlebars.CallMeOptionally.Tests;

public class OptionBehaviourTests
{
    private readonly OptionCases _optionCases = new();
    
    [Fact]
    public void IsSome_ShouldBeTrue_IfValuePresent_EvenIfDefault()
    {
        _optionCases.String.IsSome.Should().BeTrue();
        _optionCases.Char.IsSome.Should().BeTrue();
        _optionCases.Int.IsSome.Should().BeTrue();
        _optionCases.Bool.IsSome.Should().BeTrue();
        _optionCases.Model.IsSome.Should().BeTrue();
        _optionCases.UnderlyingModel.IsSome.Should().BeTrue();
    }
    
    [Fact]
    public void IsSome_ShouldBeFalse_IfNoValuePresent_OrNullGiven()
    {
        _optionCases.Null.IsSome.Should().BeFalse();
        _optionCases.None.IsSome.Should().BeFalse();
    }
    
    [Fact]
    public void Value_ShouldThrowNullRefError_IfOptionIsNone()
    {
        _optionCases.None.Invoking(obj => obj.Value).Should().ThrowExactly<UnhandledNoneException>();
        _optionCases.Null.Invoking(obj => obj.Value).Should().ThrowExactly<UnhandledNoneException>();
    }
    
    [Fact]
    public void Value_ShouldReturnUnderlyingValue_IfOptionIsSome()
    {
        _optionCases.String.Value.Should().Be("test");
        _optionCases.Char.Value.Should().Be('t');
        _optionCases.Bool.Value!.Value.Should().BeTrue();
        _optionCases.Int.Value.Should().Be(default);
        _optionCases.Model.Value.Should().BeOfType<TestModel>();
        _optionCases.Model.Value.TestStr.Should().NotBeNullOrEmpty();
        _optionCases.UnderlyingModel.Value.Should().BeOfType<Option<TestModel>>();
    }
    
}