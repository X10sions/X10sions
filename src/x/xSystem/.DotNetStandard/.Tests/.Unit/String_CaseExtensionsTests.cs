using FluentAssertions;

namespace System.Tests {
  public class String_CaseExtensionsTests {

    public const string Url_CamelCase = "api/users/32/someActionToDo?param=%a%";
    public const string Url_KebabCase = "api/users/32/some-action-to-do?param=%a%";
    public const string Url_PascalCase = "Api/Users/32/SomeActionToDo?Param=%A%";
    public const string Url_SnakeCase = "api/users/32/some_action_to_do?param=%a%";
    public const string Url_SnakeCase2 = "api/users/32/Some_Action_to_do?param=%a%";
    public const string Url_TitleCase = "Api/Users/32/Some-Action-To-Do?Param=%A%";
    public const string Url_TrainCase = "Api/Users/32/Some-Action-To-Do?Param=%A%";
    public const string Url_TrainCase2 = "api/users/32/Some-Action-To-Do?Param=%a%";

    public static TheoryData<string> UrlTestData => new TheoryData<string>   {
      Url_CamelCase,
      Url_KebabCase,
      Url_PascalCase,
      Url_SnakeCase,
      Url_SnakeCase2,
      Url_TitleCase,
      Url_TrainCase,
      Url_TrainCase2
    };

    public const string Sentence_AnyCase1 = "The quick brown fox named Jim.";
    public const string Sentence_AnyCase2 = "ThE Quick bRown FOX nAmeD jIM.";
    public const string Sentence_CamelCase = "theQuickBrownFoxNamedJim.";
    public const string Sentence_KebabCase = "the-quick-brown-fox-named-jim.";
    public const string Sentence_PascalCase = "TheQuickBrownFoxNamedJim.";
    public const string Sentence_SnakeCase = "the_quick_brown_fox_named_jim.";
    public const string Sentence_TitleCase = "The Quick Brown Fox Named Jim.";
    public const string Sentence_TrainCase = "The-Quick-Brown-Fox-Named-Jim.";

    public static TheoryData<string> SentenceTestData => new TheoryData<string> {
       Sentence_AnyCase1,
       Sentence_AnyCase2,
       Sentence_CamelCase,
       Sentence_KebabCase,
       Sentence_PascalCase,
       Sentence_SnakeCase,
       Sentence_TitleCase,
       Sentence_TrainCase
    };

    public static class Messages {
      public static string ArgumentNullException = new ArgumentNullException("source").Message;
    }

    //ToCamelCase
    [Theory, MemberData(nameof(SentenceTestData))] public void ToCamelCase_ReturnSentence(string s) => s.ToCamelCase().Should().Be(Sentence_CamelCase);
    [Theory, MemberData(nameof(UrlTestData))] public void ToCamelCase_ReturnUrl(string s) => s.ToCamelCase().Should().Be(Url_CamelCase);

    //ToKebabCase
    [Theory, MemberData(nameof(SentenceTestData))] public void ToKebabCase_ReturnSentence(string s) => s.ToKebabCase().Should().Be(Sentence_KebabCase);
    [Theory, MemberData(nameof(UrlTestData))] public void ToKebabCase_ReturnUrl(string s) => s.ToKebabCase().Should().Be(Url_KebabCase);

    //ToPascalCase
    [Theory, MemberData(nameof(SentenceTestData))] public void ToPascalCase_ReturnSentence(string s) => s.ToPascalCase().Should().Be(Sentence_PascalCase);
    [Theory, MemberData(nameof(UrlTestData))] public void ToPascalCase_ReturnUrl(string s) => s.ToPascalCase().Should().Be(Url_PascalCase);

    //ToSnakeCase
    [Theory, MemberData(nameof(SentenceTestData))] public void ToSnakeCase_ReturnSentence(string s) => s.ToSnakeCase().Should().Be(Sentence_SnakeCase);
    [Theory, MemberData(nameof(UrlTestData))] public void ToSnakeCase_ReturnUrl(string s) => s.ToSnakeCase().Should().Be(Url_SnakeCase);

    ////ToTitleCase
    //[Theory, MemberData(nameof(SentenceTestData))] public void ToTitleCase_ReturnSentence(string s) => Assert.Equal(Sentence_TitleCase, s.ToTitleCase());
    //[Theory, MemberData(nameof(UrlTestData))] public void ToTitleCase_ReturnUrl(string s) => Assert.Equal(Url_TitleCase, s.ToTitleCase());

    ////ToTrainCase
    //[Theory, MemberData(nameof(SentenceTestData))] public void ToTrainCase_ReturnSentence(string s) => Assert.Equal(Sentence_TrainCase, s.ToTrainCase(), true);
    //[Theory, MemberData(nameof(UrlTestData))] public void ToTrainCase_ReturnUrl(string s) => Assert.Equal(Url_TrainCase, s.ToTrainCase());

  }
}