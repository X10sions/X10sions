﻿using System;

namespace Test {
  public static class Assert {
    public static void Equal(object expected, object actual) {
      if (!Equals(expected, actual)) {
        throw new TestFailureException(string.Format("Assert failure - expected: {0} actual: {1}", expected, actual));
      }
    }

    public static void Equal(double expected, double actual, double epsilon) {
      if (!(actual >= expected - epsilon && actual <= expected + epsilon)) {
        throw new TestFailureException(string.Format("Assert failure - expected: {0} +/- {1} actual: {1}", expected, epsilon, actual));
      }
    }

    public static void NotEqual(object notExpected, object actual) {
      if (object.Equals(notExpected, actual)) {
        throw new TestFailureException(string.Format("Assert failure - value not expected: {0}", actual));
      }
    }

    public static void Throws<TException>(Action action) where TException : Exception {
      Exception caught = null;
      try {
        action();
      } catch (Exception e) {
        caught = e;
      }
      if (caught == null) {
        throw new TestFailureException(string.Format("Assert Failure - expected: exception of type '{0}'  actual: no exception.", typeof(TException).Name));
      } else if (!typeof(TException).IsAssignableFrom(caught.GetType())) {
        throw new TestFailureException(string.Format("Assert Failure - expected: exception of type '{0}'  actual: exception of type '{1}'", typeof(TException).Name, caught.GetType().Name));
      }
    }

  }
}