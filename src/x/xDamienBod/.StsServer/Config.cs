﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace xDamienBod.StsServer {
  public static class Config {

    public static IEnumerable<IdentityResource> GetIdentityResources() {
      return new IdentityResource[] {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
      };
    }

    public static IEnumerable<ApiResource> GetApis() {
      return new ApiResource[] {
        new ApiResource("scope_used_for_hybrid_flow", "Mvc Hybrid Client"),
        new ApiResource("native_api", "Native Client API") {
          ApiSecrets = {
            new Secret("native_api_secret".Sha256())
          }
        }
      };
    }

    public static IEnumerable<Client> GetClients() {
      return new[] {
        // MVC client using hybrid flow
        new Client {
          ClientId = "hybridclient",
          ClientName = "MVC Client",
          AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
          ClientSecrets = { new Secret("hybrid_flow_secret".Sha256()) },
          RedirectUris = { "https://localhost:44381/signin-oidc" },
          FrontChannelLogoutUri = "https://localhost:44381/signout-oidc",
          PostLogoutRedirectUris = { "https://localhost:44381/signout-callback-oidc" },
          AllowOfflineAccess = true,
          AllowedScopes = { "openid", "profile", "offline_access",  "scope_used_for_hybrid_flow" }
        },
        new Client {
          ClientId = "native.code",
          ClientName = "Native Client (Code with PKCE)",
          RedirectUris = { "http://127.0.0.1:45656" },
          PostLogoutRedirectUris = { "http://127.0.0.1:45656" },
          RequireClientSecret = false,
          AllowedGrantTypes = GrantTypes.Code,
          RequirePkce = true,
          AllowedScopes = { "openid", "profile", "email", "native_api" },
          AllowOfflineAccess = true,
          RefreshTokenUsage = TokenUsage.ReUse
        }
      };
    }

  }
}