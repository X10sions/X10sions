namespace CommonOrm {
  public interface IHaveDiscriminatorExpression<T> {
    OrmDiscriminator<T, object> Discriminator { get; set; }
  }

}