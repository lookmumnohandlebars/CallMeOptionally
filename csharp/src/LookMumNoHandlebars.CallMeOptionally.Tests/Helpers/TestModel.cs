using AutoFixture;

namespace LookMumNoHandlebars.CallMeOptionally.Tests.Helpers;


public class TestModel
{
    public TestModel()
    {
        var fixture = new Fixture();
        TestInt = fixture.Create<int>();
        TestStr = fixture.Create<string>();
    }
    public int TestInt { get; }
    public string TestStr { get; }
}