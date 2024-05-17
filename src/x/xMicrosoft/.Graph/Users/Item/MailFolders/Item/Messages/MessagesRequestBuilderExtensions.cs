using Microsoft.Graph.Models;

namespace Microsoft.Graph.Users.Item.MailFolders.Item.Messages;
  public static class MessagesRequestBuilderExtensions {

    public async static Task<List<Message>> GetMessages(this MessagesRequestBuilder builder, string filter, CancellationToken cancellationToken) {
      var repsonse = await builder.GetAsync(requestConfiguration => {
        requestConfiguration.QueryParameters.Filter = filter;
      }, cancellationToken: cancellationToken);
      return repsonse?.Value ?? new();
    }

}