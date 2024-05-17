using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.MailFolders.Item;
using Microsoft.Graph.Users.Item.MailFolders.Item.Messages;

namespace Microsoft.Graph.Users.Item;

public static class UserItemRequestBuilderExtensions {

  public async static Task<MailFolder> GetMailFolder(this UserItemRequestBuilder builder, WellKnownFolder folder, CancellationToken cancellationToken)
    => await builder.MailFolders[folder.ToString()].GetAsync(cancellationToken: cancellationToken);

  public static MailFolderItemRequestBuilder GetMailFolderItemRequestBuilder(this UserItemRequestBuilder builder, WellKnownFolder folder) => builder.MailFolders[folder.ToString()];

  public async static Task<List<Message>> GetMessages(this UserItemRequestBuilder builder, WellKnownFolder folder, string filter, CancellationToken cancellationToken)
    => await builder.MailFolders[folder.ToString()].GetMessages(filter, cancellationToken);

}
