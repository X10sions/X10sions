using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.MailFolders.Item.Messages;

namespace Microsoft.Graph.Users.Item.MailFolders.Item;
public static class MailFolderItemRequestBuilderExtensions {

  public async static Task<List<Message>> GetMessages(this MailFolderItemRequestBuilder builder, string filter, CancellationToken cancellationToken)
    => await builder.Messages.GetMessages(filter, cancellationToken);

}