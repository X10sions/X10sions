using Microsoft.Graph.Users.Item.MailFolders.Item.Messages;
using Microsoft.Graph.Users.Item.MailFolders.Item.Messages.Item.Attachments;

namespace Microsoft.Graph.Models;

public static class MessageExtensions {
  public const string ExpandMessageSize = "singleValueExtendedProperties($filter = id eq 'long 0x0E08')";
  public async static Task<List<Attachment>> GetAttachments(this Message message, MessagesRequestBuilder messagesBuilder, AttachmentsRequestBuilder.AttachmentsRequestBuilderGetQueryParameters? queryParameters = null) {
    if (message.HasAttachments == false) return new();
    var response = await messagesBuilder[message.Id]?.Attachments?.GetAsync(requestConfiguration => {
      if (queryParameters is not null) {
        requestConfiguration.QueryParameters = queryParameters;
      }
    });
    return response?.Value ?? new();
  }

  public async static Task<List<Attachment>> GetAttachmentsNameSize(this Message message, MessagesRequestBuilder messagesBuilder)
    => await message.GetAttachments(messagesBuilder, new AttachmentsRequestBuilder.AttachmentsRequestBuilderGetQueryParameters {
      Select = [nameof(Attachment.Name), nameof(Attachment.Size)]
    });

}
