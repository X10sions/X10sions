using Raven.Client.Documents;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public static class RavenDbDocumentStoreHolder {

    // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-ravendb/db/

    // https://stackoverflow.com/questions/43115547/setting-up-dotnet-core-with-ravendb
    // https://ravendb.net/docs/article-page/4.2/csharp/client-api/creating-document-store

    // https://github.com/JudahGabriel/RavenDB.DependencyInjection
    // https://github.com/FriendlyAgent/RavenDB.AspNetCore.DependencyInjection

    private static readonly Lazy<IDocumentStore> DocStore = new Lazy<IDocumentStore>(CreateNorthwindStore);
    private static readonly Lazy<IDocumentStore> MediaDocStore = new Lazy<IDocumentStore>(CreateMediaStore);

    public static IDocumentStore Store => DocStore.Value;
    public static IDocumentStore MediaStore => MediaDocStore.Value;

    public static string NorthwindDatabaseName { get; set; }
    public static string MediaDatabaseName { get; set; }

    public static string ConnectionStringName { get; set; }

    public static void xSetDbInfo(string connectionStringName, string dbNameNorthwind, string dbNameMedia) {
      ConnectionStringName = connectionStringName;
      NorthwindDatabaseName = dbNameNorthwind;
      MediaDatabaseName = dbNameMedia;
    }

    private static IDocumentStore CreateNorthwindStore() => new DocumentStore {
      Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),
      Conventions = {
        MaxNumberOfRequestsPerSession = 10,
        UseOptimisticConcurrency = true
      },
      Database = NorthwindDatabaseName,
      Urls = new[] {
        "http://your_RavenDB_cluster_node"
      }
    }.Initialize();

    private static IDocumentStore CreateMediaStore() => new DocumentStore {
      Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),
      Conventions = {
        MaxNumberOfRequestsPerSession = 10,
        UseOptimisticConcurrency = true
      },
      Database = MediaDatabaseName,
      Urls = new[] {
        "http://your_RavenDB_cluster_node"
      }
    }.Initialize();
  }

}
