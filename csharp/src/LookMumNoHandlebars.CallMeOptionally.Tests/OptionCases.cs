namespace LookMumNoHandlebars.CallMeOptionally.Tests;

public class OptionCases
{
    public Option<string> String = Option.Some("test");
    public Option<char> Char = Option.Some('t');
    public Option<int> Int = new(default);
    public Option<bool?> Bool = new(true);
    public Option<TestModel> Model = Option.Some(new TestModel());
    public Option<Option<TestModel>> UnderlyingModel = Option.Some(new Option<TestModel>(new TestModel()));
    public Option<string> None = Option.None<string>();
    public Option<TestModel> Null = new(null!);
}