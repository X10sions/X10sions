using System;

namespace Test {
  public class TestFailureException : Exception {
    public TestFailureException(string message)
        : base(message) {
    }
  }
}

