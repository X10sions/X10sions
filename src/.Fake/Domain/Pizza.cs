namespace X10sions.Fake.Domain;

public class Pizza {
  public Pizza(Size size, DoughType dough, SauceType sauce, IReadOnlyCollection<ToppingType>? toppings = null, HerbType? herbs = null) {
    Size = size;
    Dough = dough;
    Sauce = sauce;
    Toppings = toppings;
    Herbs = herbs ?? HerbType.None;
  }
  public Size Size { get; }
  public DoughType Dough { get; }
  public SauceType Sauce { get; }
  public HerbType Herbs { get; }
  public IReadOnlyCollection<ToppingType>? Toppings { get; }
}

public class PizzaBuilder {
  private Size? _size;
  private DoughType? _dough;
  private SauceType? _sauce;
  private HerbType? _herbs;
  private List<ToppingType> _toppings = new();
  public PizzaBuilder(Size size) => _size = size;

  public Pizza Build() => new(
    _size ?? throw new ArgumentNullException(nameof(_size)),
    _dough ?? throw new ArgumentNullException(nameof(_dough)),
    _sauce ?? throw new ArgumentNullException(nameof(_sauce)),
    _toppings,
    _herbs ?? throw new ArgumentNullException(nameof(_herbs))
    );

  public PizzaBuilder WithDough(DoughType dough) {
    _dough = dough;
    return this;
  }

  public PizzaBuilder WithSauce(SauceType sauce) {
    _sauce = sauce;
    return this;
  }

  public PizzaBuilder WithHerb(HerbType herbs) {
    _herbs = herbs;
    return this;
  }

  public PizzaBuilder WithTopping(ToppingType topping) {
    _toppings.Add(topping);
    return this;
  }

  public static implicit operator Pizza(PizzaBuilder builder) => builder.Build();
}

public class MargheritaPizzaDirector {
  public void MakePizza(PizzaBuilder builder) {
    builder.WithDough(DoughType.ThinCrust);
    builder.WithSauce(SauceType.TomoatoBasil);
    builder.WithTopping(ToppingType.MozzarellaCheese);
    builder.WithHerb(HerbType.Basil);
  }
}
public class InvalidPizzaDirector {
  public void MakePizza(PizzaBuilder builder) {
    builder.WithDough(DoughType.ThinCrust);
    builder.WithSauce(SauceType.TomoatoBasil);
    builder.WithTopping(ToppingType.Pineapple);
    //builder.WithHerb(HerbType.Basil);
  }
}

public class PizzaValidator {
  public bool CanWeCookIt(Pizza pizza) => throw new NotImplementedException();
}

public readonly record struct DoughType(string value) {
  public static DoughType StuffedCrust = new("StuffedCrust");
  public static DoughType ThinCrust = new("ThinCrust");
}

public readonly record struct HerbType(string value) {
  public static HerbType Basil = new("Basil");
  public static HerbType None = new(string.Empty);
}

public readonly record struct SauceType(string value) {
  public static SauceType TomoatoBasil = new("TomoatoBasil");
}

public readonly record struct Size(string value) {
  public static Size Regular = new("Regular");
}

public readonly record struct ToppingType(string value) {
  public static ToppingType MozzarellaCheese = new("MozzarellaCheese");
  public static ToppingType Pineapple = new("Pineapple");
}
