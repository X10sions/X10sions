﻿//using System.Linq;
//using System.Security.Claims;

namespace System.Security.Principal {

  public static class IPrincipalExtensions {

    //public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value) {
    //  //https://stackoverflow.com/questions/24587414/how-to-update-a-claim-in-asp-net-identity 
    //  var identity = currentPrincipal.Identity as ClaimsIdentity;
    //  if (identity == null)
    //    return;
    //  // check for existing claim and remove it
    //  var existingClaim = identity.FindFirst(key);
    //  if (existingClaim != null)
    //    identity.RemoveClaim(existingClaim);
    //  // add new claim
    //  identity.AddClaim(new Claim(key, value));
    //  var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
    //  authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
    //}

    //public static string GetClaimValue(this IPrincipal currentPrincipal, string key) {
    //  var identity = currentPrincipal.Identity as ClaimsIdentity;
    //  if (identity == null) return null;
    //  var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
    //  return claim.Value;
    //}

  }
}
