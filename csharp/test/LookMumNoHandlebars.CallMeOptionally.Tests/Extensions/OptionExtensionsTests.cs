using FluentAssertions;
using LookMumNoHandlebars.CallMeOptionally.Extensions;
using LookMumNoHandlebars.CallMeOptionally.Tests.Helpers;

namespace LookMumNoHandlebars.CallMeOptionally.Tests.Extensions;

public class OptionExtensionsTests
{
    private OptionCases _testCases = new();
    
    [Fact]
    public void ValueOrDefault_IfSome_ShouldGiveValue() => _testCases.Int.ValueOrDefault(1).Should().Be(0);
    [Fact]
    public void ValueOrDefault_IfNone_ShouldGiveValue() => _testCases.None.ValueOrDefault("fallback").Should().Be("fallback");
    
    [Fact]
    public void ToList_IfSome_ShouldGiveSingleList() => _testCases.Int.ToList().Should().HaveCount(1).And.Contain(0);
    [Fact]
    public void ToList_IfNone_ShouldGiveEmptyList() => _testCases.None.ToList().Should().HaveCount(0);

    [Fact]
    public void ToSingleList_IfSome_ShouldGiveSingleList() =>
        _testCases.Int.ToSingleList().Should().HaveCount(1).And.Contain(_testCases.Int);
    [Fact]
    public void ToSingleList_IfNone_ShouldGiveSingleList() =>
        _testCases.None.ToSingleList().Should().HaveCount(1).And.Contain(_testCases.None);

    [Fact]
    public void Match_IfSome_ShouldGiveSomeMatchedResult() =>
        _testCases.Int.Match(val => val + 2, () => 1).Should().Be(2);

    [Fact]
    public void Match_ShouldNotMaskException() =>
        _testCases.Int.Invoking(sut => sut.Match(_ => throw new Exception(), () => 1)).Should().Throw<Exception>();
    [Fact]
    public void Match_IfNone_ShouldGiveNoneMatchedResult() =>
        _testCases.None.Match(_ => 2, () => 1).Should().Be(1);

    [Fact]
    public void Act_IfSome_ShouldPerformSomeAction()
    {
        var testVal = 1;
        _testCases.Int.Act(val => testVal += val, () => testVal += 2);
        testVal.Should().Be(1);
    }
    
    [Fact]
    public void Act_ShouldNotMaskException() =>
        _testCases.Int.Invoking(sut => sut.Act(_ => throw new Exception(), () => { })).Should().Throw<Exception>();

    
    [Fact]
    public void Act_IfNone_ShouldPerformNoneAction()
    {
        var testVal = string.Empty;
        _testCases.None.Act(val => testVal += val, () => testVal = "none");
        testVal.Should().Be("none");
    }
}