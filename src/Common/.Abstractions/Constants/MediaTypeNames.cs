using System;

namespace Common.Constants {
  public static class MediaTypeNames {
    // https://www.iana.org/assignments/media-types/media-types.xhtml

    public static class Application {
      public const string AtomcatXml = "application/atomcat+xml";
      public const string AtomXml = "application/atom+xml";
      public const string Ecmascript = "application/ecmascript";
      public const string Edifact = "application/EDIFACT";
      public const string EdiX12 = "application/EDI-X12";
      public const string FontWoff = "application/font-woff";
      public const string JavaArchive = "application/java-archive";
      public const string Javascript = "application/javascript";
      public const string Json = "application/json";
      public const string JsonPatch = "application/json-patch+json";
      public const string Mp4 = "application/mp4";
      public const string MsWord = "application/msword";
      public const string OctetStream = System.Net.Mime.MediaTypeNames.Application.Octet;
      public const string Ogg = "application/ogg";
      public const string Pdf = System.Net.Mime.MediaTypeNames.Application.Pdf;
      public const string Pkcs10 = "application/pkcs10";
      public const string Pkcs7Mime = "application/pkcs7-mime";
      public const string Pkcs7Signature = "application/pkcs7-signature";
      public const string Pkcs8 = "application/pkcs8";
      public const string Postscript = "application/postscript";
      public const string RdfXml = "application/rdf+xml";
      public const string RssXml = "application/rss+xml";
      public const string Rtf = "application/rtf";
      public const string SmilXml = "application/smil+xml";
      public const string SoapXml = System.Net.Mime.MediaTypeNames.Application.Soap;
      public const string VndGoogleEarthKmlXml = "application/vnd.google-earth.kml+xml";
      public const string VndMozillaXulXml = "application/vnd.mozilla.xul+xml";
      public const string VndMsExcel = "application/vnd.ms-excel";
      public const string VndMsPowerpoint = "application/vnd.ms-powerpoint";
      public const string VndOasisOpendocumentGraphics = "application/vnd.oasis.opendocument.graphics";
      public const string VndOasisOpendocumentPresentation = "application/vnd.oasis.opendocument.presentation";
      public const string VndOasisOpendocumentSpreadsheet = "application/vnd.oasis.opendocument.spreadsheet";
      public const string VndOasisOpendocumentText = "application/vnd.oasis.opendocument.text";
      public const string VndOpenxmlformatsOfficedocumentPresentationmlPresentation = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
      public const string VndOpenxmlformatsOfficedocumentSpreadsheetmlSheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
      public const string VndOpenxmlformatsOfficedocumentWordprocessingmlDocument = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
      public const string WwwFormUrlEncoded = "application/x-www-form-urlencoded";
      public const string XDeb = "application/x-deb";
      public const string XDvi = "application/x-dvi";
      public const string XFontOtf = "application/x-font-otf";
      public const string XFontTtf = "application/x-font-ttf";
      public const string XFontWoff = "application/x-font-woff";
      public const string XGzip = "application/x-gzip";
      public const string XhtmlXml = "application/xhtml+xml";
      public const string XJavascript = "application/x-javascript";
      public const string XLatex = "application/x-latex";
      public const string Xml = "application/xml";
      public const string XmlDtd = "application/xml-dtd";
      public const string XopXml = "application/xop+xml";
      public const string XPkcs12 = "application/x-pkcs12";
      public const string XPkcs7Certificates = "application/x-pkcs7-certificates";
      public const string XPkcs7Certreqresp = "application/x-pkcs7-certreqresp";
      public const string XPkcs7Mime = "application/x-pkcs7-mime";
      public const string XPkcs7Signature = "application/x-pkcs7-signature";
      public const string XRarCompressed = "application/x-rar-compressed";
      public const string XShockwaveFlash = "application/x-shockwave-flash";
      public const string XSilverlightApp = "application/x-silverlight-app";
      public const string XsltXml = "application/xslt+xml";
      public const string XStuffit = "application/x-stuffit";
      public const string XTar = "application/x-tar";
      public const string Zip = System.Net.Mime.MediaTypeNames.Application.Zip;
    }

    public static class Audio {
      public const string Basic = "audio/basic";
      public const string L24 = "audio/L24";
      public const string Midi = "audio/midi";
      public const string Mp4 = "audio/mp4";
      public const string Mpeg = "audio/mpeg";
      public const string Ogg = "audio/ogg";
      public const string VndRnRealaudio = "audio/vnd.rn-realaudio";
      public const string VndWave = "audio/vnd.wave";
      public const string Vorbis = "audio/vorbis";
      public const string Webm = "audio/webm";
      public const string XAac = "audio/x-aac";
      public const string XAiff = "audio/x-aiff";
      public const string XMpegurl = "audio/x-mpegurl";
      public const string XMsWax = "audio/x-ms-wax";
      public const string XMsWma = "audio/x-ms-wma";
      public const string XWav = "audio/x-wav";
    }

    public static class Image {
      public const string Bmp = "image/bmp";
      public const string Gif = System.Net.Mime.MediaTypeNames.Image.Gif;
      public const string Jpeg = System.Net.Mime.MediaTypeNames.Image.Jpeg;
      public const string Pjpeg = "image/pjpeg";
      public const string Png = "image/png";
      public const string SvgXml = "image/svg+xml";
      public const string Tiff = System.Net.Mime.MediaTypeNames.Image.Tiff;
      public const string VndMicrosoftIcon = "image/vnd.microsoft.icon";
      public const string Webp = "image/webp";
    }

    public static class Message {
      public const string Http = "message/http";
      public const string ImdnXml = "message/imdn+xml";
      public const string Partial = "message/partial";
      public const string Rfc822 = "message/rfc822";
    }

    public static class Model {
      public const string Example = "model/example";
      public const string Iges = "model/iges";
      public const string Mesh = "model/mesh";
      public const string Vrml = "model/vrml";
      public const string X3DBinary = "model/x3d+binary";
      public const string X3DVrml = "model/x3d+vrml";
      public const string X3DXml = "model/x3d+xml";
    }

    public static class Multipart {
      public const string Alternative = "multipart/alternative";
      public const string Encrypted = "multipart/encrypted";
      public const string FormData = "multipart/form-data";
      public const string Mixed = "multipart/mixed";
      public const string Related = "multipart/related";
      public const string Signed = "multipart/signed";
    }

    public static class Text {
      [Obsolete("Use application/javascript")] public const string Javascript = "text/javascript";
      public const string Cmd = "text/cmd";
      public const string Css = "text/css";
      public const string Csv = "text/csv";
      public const string Html = System.Net.Mime.MediaTypeNames.Text.Html;
      public const string Plain = System.Net.Mime.MediaTypeNames.Text.Plain;
      public const string RichText = System.Net.Mime.MediaTypeNames.Text.RichText;
      public const string Sgml = "text/sgml";
      public const string Vcard = "text/vcard";
      public const string XGwtRpc = "text/x-gwt-rpc";
      public const string XJqueryTmpl = "text/x-jquery-tmpl";
      public const string Xml = System.Net.Mime.MediaTypeNames.Text.Xml;
      public const string Yaml = "text/yaml";
    }

    public static class Video {
      public const string H264 = "video/h264";
      public const string Mp4 = "video/mp4";
      public const string Mpeg = "video/mpeg";
      public const string Ogg = "video/ogg";
      public const string Quicktime = "video/quicktime";
      public const string Threegpp = "video/3gpp";
      public const string Webm = "video/webm";
      public const string XFlv = "video/x-flv";
      public const string XMatroska = "video/x-matroska";
      public const string XMsWmv = "video/x-ms-wmv";
    }
  }
}