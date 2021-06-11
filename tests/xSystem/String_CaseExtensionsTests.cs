using Xunit;

namespace System.Tests {
  public class String_CaseExtensionsTests {

    public static TheoryData<string> UrlTestData => new TheoryData<string>   {
       "api/users/32/someActionToDo?param=%a%" ,
       "Api/Users/32/SomeActionToDo?Param=%A%" ,
       "api/users/32/some-action-to-do?param=%a%" ,
       "api/users/32/Some-Action-To-Do?Param=%a%" ,
       "api/users/32/some_action_to_do?param=%a%" ,
       "api/users/32/Some_Action_to_do?param=%a%"
    };

    public const string Sentence_ = "The quick brown fox named Jim.";
    public const string Sentence_CamelCase = "theQuickBrownFoxNamedJim.";
    public const string Sentence_KebabCase = "the-quick-brown-fox-named-Jim.";
    public const string Sentence_PascalCase = "TheQuickBrownFoxnamedJim.";
    public const string Sentence_SnakeCase = "the_quick_brown_fox_named_jim.";
    public const string Sentence_TrainCase = "The-Quick-Brown-Fox-Named-Jim.";

    public static TheoryData<string> SentenceTestData => new TheoryData<string> {
       Sentence_,
       Sentence_CamelCase ,
       Sentence_KebabCase ,
       Sentence_PascalCase,
       Sentence_SnakeCase ,
       Sentence_TrainCase
    };

    public static class Messages {
      public static string ArgumentNullException = new ArgumentNullException("source").Message;
    }

    //[Theory, MemberData(nameof(TestDataList))]

    //ToCamelCase
    [Theory, InlineData(null)] public void xToCamelCase_ReturnArgumentNullException(string s) => Assert.Throws<ArgumentNullException>(() => s.xToCamelCase());
    [Theory, MemberData(nameof(SentenceTestData))] public void xToCamelCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.xToCamelCase());
    [Theory, MemberData(nameof(UrlTestData))] public void xToCamelCase_ReturnUrl(string s) => Assert.Equal("api/users/32/someActionToDo?param=%a%", s.xToCamelCase());

    [Theory, MemberData(nameof(SentenceTestData))] public void ToCamelCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.ToCamelCase());
    [Theory, MemberData(nameof(UrlTestData))] public void ToCamelCase_ReturnUrl(string s) => Assert.Equal("api/users/32/someActionToDo?param=%a%", s.ToCamelCase());

    //ToKebabCase
    [Theory, InlineData(null)] public void xToKebabCase_ReturnArgumentNullException(string s) => Assert.Throws<ArgumentNullException>(() => s.xToKebabCase());
    [Theory, MemberData(nameof(SentenceTestData))] public void xToKebabCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.xToKebabCase());
    [Theory, MemberData(nameof(UrlTestData))] public void xToKebabCase_ReturnUrl(string s) => Assert.Equal("api/users/32/some-action-to-do?param=%a%", s.xToKebabCase());

    //ToPascalCase
    [Theory, InlineData(null)] public void xToPascalCasec_ReturnArgumentNullException(string s) => Assert.Throws<ArgumentNullException>(() => s.xToPascalCase());
    [Theory, MemberData(nameof(SentenceTestData))] public void xToPascalCasec_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.xToPascalCase());
    [Theory, MemberData(nameof(UrlTestData))] public void xToPascalCasec_ReturnUrl(string s) => Assert.Equal("Api/Users/32/SomeActionToDo?Param=%A%", s.xToPascalCase());

    [Theory, MemberData(nameof(SentenceTestData))] public void ToPascalCasec_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.ToPascalCase());
    [Theory, MemberData(nameof(UrlTestData))] public void ToPascalCasec_ReturnUrl(string s) => Assert.Equal("Api/Users/32/SomeActionToDo?Param=%A%", s.ToPascalCase());

    //ToSnakeCase
    [Theory, InlineData(null)] public void xToSnakeCase_ReturnArgumentNullException(string s) => Assert.Throws<ArgumentNullException>(() => s.xToSnakeCase());
    [Theory, MemberData(nameof(SentenceTestData))] public void xToSnakeCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.xToSnakeCase());
    [Theory, MemberData(nameof(UrlTestData))] public void xToSnakeCase_ReturnUrl(string s) => Assert.Equal("api/users/32/some_action_to_do?param=%a%", s.xToSnakeCase());

    [Theory, MemberData(nameof(SentenceTestData))] public void ToSnakeCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.ToSnakeCase());
    [Theory, MemberData(nameof(UrlTestData))] public void ToSnakeCase_ReturnUrl(string s) => Assert.Equal("api/users/32/some_action_to_do?param=%a%", s.ToSnakeCase());

    //ToTitleCase
    [Theory, MemberData(nameof(SentenceTestData))] public void ToTitleCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.ToTitleCase());
    [Theory, MemberData(nameof(UrlTestData))] public void ToTitleCase_ReturnUrl(string s) => Assert.Equal("Api/Users/32/Some-Action-To-Do?Param=%A%", s.ToTitleCase());

    //ToTrainCase
    [Theory, InlineData(null)] public void xToTrainCase_ReturnArgumentNullException(string s) => Assert.Throws<ArgumentNullException>(() => s.xToTrainCase());
    [Theory, MemberData(nameof(SentenceTestData))] public void xToTrainCase_ReturnSentence(string s) => Assert.Equal(Sentence_CamelCase, s.xToTrainCase());
    [Theory, MemberData(nameof(UrlTestData))] public void xToTrainCase_ReturnUrl(string s) => Assert.Equal("Api/Users/32/Some-Action-To-Do?Param=%A%", s.xToTrainCase());

  }
}