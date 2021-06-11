namespace System.Net.Mail {
  public static class MailAddressCollectionExtensions {

    public static MailAddressCollection Add(this MailAddressCollection coll, MailAddress[] addresses) {
      if (addresses != null) {
        foreach (var a in addresses) {
          coll.Add(a);
        }
      }
      return coll;
    }

    //public static MailAddressCollection Add(this MailAddressCollection coll, string[] addresses) {
    //  coll.Add(addresses);
    //  if (addresses != null) {
    //    foreach (var s in from x in addresses where !string.IsNullOrWhiteSpace(x) select x) {
    //      coll.Add(s);
    //    }
    //  }
    //  return coll;
    //}

  }
}
