using System.Linq.Expressions;
//using FastExpressionCompiler;
//using FastExpressionCompiler.LightExpression;
//using static FastExpressionCompiler.LightExpression.Expression;

namespace Common.Linq;

public class CompiledExpression<T> {
  public CompiledExpression(Expression<Func<T, bool>> predicateExpr) {
    Expression = predicateExpr;
    Func = predicateExpr.Compile();
    //Func = predicateExpr.CompileFast();
    //  predicateExpr.to
  }

//  public CompiledExpression(Func<T, bool> predicateFunc) {
//    Expression = x => predicateFunc(x);
//    Func = predicateFunc;
//  }

  public bool Value(T arg) => Func(arg);
  public Expression<Func<T, bool>> Expression { get; }
  public Func<T, bool> Func { get; }
}

//public class xxComputedBool<T> : xxComputed<T, bool> {
//  public xxComputedBool(bool value) : base(value) { }
//}

