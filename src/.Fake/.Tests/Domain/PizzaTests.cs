using FluentAssertions;

namespace X10sions.Fake.Domain;
public class PizzaTests(PizzaValidator _pizzaValidator) {

  [Fact]
  public void Cannot_cook_pizza_without_toppings() {
    //Arrange
    var pizza = new PizzaBuilder(Size.Regular).WithDough(DoughType.StuffedCrust).WithSauce(SauceType.TomoatoBasil).Build();
    //Act
    var canWeCookIt = _pizzaValidator.CanWeCookIt(pizza);
    //Assert
    canWeCookIt.Should().BeFalse();
  }

  [Fact]
  public void Can_cook_a_marghertita() {
    //Arrange
    var pizzaBuilder = new PizzaBuilder(Size.Regular);
    new MargheritaPizzaDirector().MakePizza(pizzaBuilder);
    //Act
    var canWeCookIt = _pizzaValidator.CanWeCookIt(pizzaBuilder);
    //Assert
    canWeCookIt.Should().BeTrue();
  }

  [Fact]
  public void Cannot_cook_a_pineapple_pizza() {
    //Arrange
    var pizzaBuilder = new PizzaBuilder(Size.Regular);
    new InvalidPizzaDirector().MakePizza(pizzaBuilder);
    //Act
    var canWeCookIt = _pizzaValidator.CanWeCookIt(pizzaBuilder);
    //Assert
    canWeCookIt.Should().BeFalse();
  }

}
