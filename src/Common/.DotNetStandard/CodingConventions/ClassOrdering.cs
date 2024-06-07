using Common.Features.DummyFakeExamples.User;

namespace Common.CodingConventions;// Match folder structure
public class ClassOrdering {//match class names with class files
  private static readonly string HeaderName = "X-API-Key";
  private readonly IUserService _userService;
  public ClassOrdering(IUserService userService) => _userService = userService;
  ~ClassOrdering() { }
  public delegate string SomeDelegate(string value);
  public event EventHandler SomeEvent;
  public string SomeProperty { get; set; }
  public void SomePublicMethod() { }
  //public async Task<User> GetUserAsync() => await _userService.GetByIdAsync(Guid.NewGuid());
  internal void SomeInternalMethod() { }
  protected virtual void OnSomeEvent() => SomeEvent?.Invoke(this, EventArgs.Empty);
  private void SomePrivateMethod() { }

}