using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http {
  public class StatusCodesDictionary : Dictionary<int, string> {

    public static StatusCodesDictionary Instance => new StatusCodesDictionary();

    public StatusCodesDictionary() {
      this[StatusCodes.Status100Continue] = "Continue";
      this[StatusCodes.Status101SwitchingProtocols] = "SwitchingProtocols";
      this[StatusCodes.Status102Processing] = "Processing";
      this[StatusCodes.Status200OK] = "OK";
      this[StatusCodes.Status201Created] = "Created";
      this[StatusCodes.Status202Accepted] = "Accepted";
      this[StatusCodes.Status203NonAuthoritative] = "NonAuthoritative";
      this[StatusCodes.Status204NoContent] = "NoContent";
      this[StatusCodes.Status205ResetContent] = "ResetContent";
      this[StatusCodes.Status206PartialContent] = "PartialContent";
      this[StatusCodes.Status207MultiStatus] = "MultiStatus";
      this[StatusCodes.Status208AlreadyReported] = "AlreadyReported";
      this[StatusCodes.Status226IMUsed] = "IMUsed";
      this[StatusCodes.Status300MultipleChoices] = "MultipleChoices";
      this[StatusCodes.Status301MovedPermanently] = "MovedPermanently";
      this[StatusCodes.Status302Found] = "Found";
      this[StatusCodes.Status303SeeOther] = "SeeOther";
      this[StatusCodes.Status304NotModified] = "NotModified";
      this[StatusCodes.Status305UseProxy] = "UseProxy";
      this[StatusCodes.Status306SwitchProxy] = "SwitchProxy";
      this[StatusCodes.Status307TemporaryRedirect] = "TemporaryRedirect";
      this[StatusCodes.Status308PermanentRedirect] = "PermanentRedirect";
      this[StatusCodes.Status400BadRequest] = "BadRequest";
      this[StatusCodes.Status401Unauthorized] = "Unauthorized";
      this[StatusCodes.Status402PaymentRequired] = "PaymentRequired";
      this[StatusCodes.Status403Forbidden] = "Forbidden";
      this[StatusCodes.Status404NotFound] = "NotFound";
      this[StatusCodes.Status405MethodNotAllowed] = "MethodNotAllowed";
      this[StatusCodes.Status406NotAcceptable] = "NotAcceptable";
      this[StatusCodes.Status407ProxyAuthenticationRequired] = "ProxyAuthenticationRequired";
      this[StatusCodes.Status408RequestTimeout] = "RequestTimeout";
      this[StatusCodes.Status409Conflict] = "Conflict";
      this[StatusCodes.Status410Gone] = "Gone";
      this[StatusCodes.Status411LengthRequired] = "LengthRequired";
      this[StatusCodes.Status412PreconditionFailed] = "PreconditionFailed";
      this[StatusCodes.Status413PayloadTooLarge] = "PayloadTooLarge";
      this[StatusCodes.Status413RequestEntityTooLarge] = "RequestEntityTooLarge";
      this[StatusCodes.Status414RequestUriTooLong] = "RequestUriTooLong";
      this[StatusCodes.Status414UriTooLong] = "UriTooLong";
      this[StatusCodes.Status415UnsupportedMediaType] = "UnsupportedMediaType";
      this[StatusCodes.Status416RangeNotSatisfiable] = "RangeNotSatisfiable";
      this[StatusCodes.Status416RequestedRangeNotSatisfiable] = "RequestedRangeNotSatisfiable";
      this[StatusCodes.Status417ExpectationFailed] = "ExpectationFailed";
      this[StatusCodes.Status418ImATeapot] = "ImATeapot";
      this[StatusCodes.Status419AuthenticationTimeout] = "AuthenticationTimeout";
      this[StatusCodes.Status421MisdirectedRequest] = "MisdirectedRequest";
      this[StatusCodes.Status422UnprocessableEntity] = "UnprocessableEntity";
      this[StatusCodes.Status423Locked] = "Locked";
      this[StatusCodes.Status424FailedDependency] = "FailedDependency";
      this[StatusCodes.Status426UpgradeRequired] = "UpgradeRequired";
      this[StatusCodes.Status428PreconditionRequired] = "PreconditionRequired";
      this[StatusCodes.Status429TooManyRequests] = "TooManyRequests";
      this[StatusCodes.Status431RequestHeaderFieldsTooLarge] = "RequestHeaderFieldsTooLarge";
      this[StatusCodes.Status451UnavailableForLegalReasons] = "UnavailableForLegalReasons";
      this[StatusCodes.Status500InternalServerError] = "InternalServerError";
      this[StatusCodes.Status501NotImplemented] = "NotImplemented";
      this[StatusCodes.Status502BadGateway] = "BadGateway";
      this[StatusCodes.Status503ServiceUnavailable] = "ServiceUnavailable";
      this[StatusCodes.Status504GatewayTimeout] = "GatewayTimeout";
      this[StatusCodes.Status505HttpVersionNotsupported] = "HttpVersionNotsupported";
      this[StatusCodes.Status506VariantAlsoNegotiates] = "VariantAlsoNegotiates";
      this[StatusCodes.Status507InsufficientStorage] = "InsufficientStorage";
      this[StatusCodes.Status508LoopDetected] = "LoopDetected";
      this[StatusCodes.Status510NotExtended] = "NotExtended";
      this[StatusCodes.Status511NetworkAuthenticationRequired] = "NetworkAuthenticationRequired";
    }

  }
}