using Microsoft.Graph.Users.Item.Messages.Item.Move;
using Microsoft.Graph.Models;

namespace Microsoft.Graph;
public static class GraphServiceClientExtensions {

  /// <summary> https://learn.microsoft.com/en-us/graph/api/message-delete?view=graph-rest-1.0&tabs=http</summary>
  public async static Task DeleteUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].DeleteAsync(cancellationToken: cancellationToken);

  //public static async Task<MessageCollectionResponse?> GetInboxMessages(this GraphServiceClient graphServiceClient, EmailAddress emailAddress)
  //  => await graphServiceClient.Users[emailAddress.Address].MailFolders[Constants.Url.Inbox].Messages
  //                          //.Expand("attachments")
  //                          //   .Top(20)
  //                          .GetAsync();

  /// <summary> https://learn.microsoft.com/en-us/graph/api/message-move?view=graph-rest-1.0</summary>
  public async static Task<Message> MoveUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, string destinationFolderId, CancellationToken cancellationToken)
      => await graphServiceClient.Users[user.Id].Messages[message.Id].Move.PostAsync(new MovePostRequestBody {
        DestinationId = destinationFolderId,
      }, cancellationToken: cancellationToken);

  public async static Task<Message> MoveUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, WellKnownFolder destination, CancellationToken cancellationToken)
      => await graphServiceClient.Users[user.Id].Messages[message.Id].Move.PostAsync(new MovePostRequestBody {
        DestinationId = destination.ToString(),
      }, cancellationToken: cancellationToken);

  public async static Task<Message> MoveUserMessageToDeletedItemsAsync(this GraphServiceClient graphServiceClient, User user, Message message, CancellationToken cancellationToken)
    => await graphServiceClient.MoveUserMessageAsync(user, message, Constants.Url.DeletedItems, cancellationToken);

  /// <summary>https://learn.microsoft.com/en-us/graph/api/message-update?view=graph-rest-1.0&tabs=csharp</summary>
  public async static Task<Message> UpdateUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, Message newMessageBody, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].PatchAsync(newMessageBody, cancellationToken: cancellationToken);

  public async static Task<Message> UpdateUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, Func<Message> newMessageBody, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].PatchAsync(newMessageBody(), cancellationToken: cancellationToken);

}