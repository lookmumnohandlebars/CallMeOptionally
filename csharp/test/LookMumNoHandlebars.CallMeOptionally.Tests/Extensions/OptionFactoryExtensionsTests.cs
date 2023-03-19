using FluentAssertions;
using LookMumNoHandlebars.CallMeOptionally.Extensions;
using LookMumNoHandlebars.CallMeOptionally.Tests.Helpers;

namespace LookMumNoHandlebars.CallMeOptionally.Tests.Extensions;

public class OptionFactoryExtensionsTests
{
    [Fact]
    public void ToOption_ShouldConvertValueToSome()
    {
        var sut = new TestModel().ToOption();
        sut.Should().BeOfType<Option<TestModel>>();
        sut.IsSome.Should().BeTrue();
    }

    [Fact]
    public void ToOption_ShouldConvertNullToNone()
    {
        TestModel? nullTestModel = null;
        var sut = nullTestModel.ToOption();
        sut.Should().BeOfType<Option<TestModel>>();
        sut.IsSome.Should().BeFalse();
    }
    
    [Fact]
    public void ToOption_WithMapFunc_ShouldConvertToSomewithMappedValue()
    {
        var sut = new TestModel().ToOption(val => new Range(val.TestInt, 100));
        sut.Should().BeOfType<Option<Range>>();
        sut.IsSome.Should().BeTrue();
    }
    
    [Fact]
    public void ToOption_ShouldConvertNullToNoneWhenMapFuncIncluded()
    {
        TestModel? nullTestModel = null;
        var sut = nullTestModel.ToOption(val => new Range(val.TestInt, 100));
        sut.Should().BeOfType<Option<Range>>();
        sut.IsSome.Should().BeFalse();
    }
}