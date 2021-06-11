namespace Common.Json.Serialization.ContractResolvers {

  public class SnakeCasePropertyNamesContractResolver : DeliminatorSeparatedPropertyNamesContractResolver {
    public SnakeCasePropertyNamesContractResolver() : base('_') {
    }
  }

}