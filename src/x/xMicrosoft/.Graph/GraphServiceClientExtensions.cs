using Microsoft.Graph.Users.Item.Messages.Item.Move;
using Microsoft.Graph.Models;

namespace Microsoft.Graph;

public static class GraphServiceClientExtensions {

  /// <summary> https://learn.microsoft.com/en-us/graph/api/message-delete?view=graph-rest-1.0&tabs=http</summary>
  public async static Task DeleteUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].DeleteAsync(cancellationToken: cancellationToken);

  public async static Task<MailFolder> GetMailFolder(this GraphServiceClient graphServiceClient, User user, WellKnownFolder folder, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].MailFolders[folder.ToString()].GetAsync(cancellationToken: cancellationToken);

  /// <summary> https://learn.microsoft.com/en-us/graph/api/message-move?view=graph-rest-1.0</summary>
  public async static Task<Message> MoveUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, string destinationFolderId, CancellationToken cancellationToken)
      => await graphServiceClient.Users[user.Id].Messages[message.Id].Move.PostAsync(new MovePostRequestBody {
        DestinationId = destinationFolderId
      }, cancellationToken: cancellationToken);

  public async static Task<Message> MoveUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, WellKnownFolder destination, CancellationToken cancellationToken)
    => await graphServiceClient.MoveUserMessageAsync(user, message, destination.ToString(), cancellationToken);

  public async static Task MoveUserMessagesAsync(this GraphServiceClient graphServiceClient, User user, IEnumerable<Message> messages, string destinationFolderId, CancellationToken cancellationToken) {
    foreach (var message in messages) {
      await graphServiceClient.MoveUserMessageAsync(user, message, destinationFolderId, cancellationToken);
    }
  }

  public async static Task MoveUserMessagesAsync(this GraphServiceClient graphServiceClient, User user, IEnumerable<Message> messages, WellKnownFolder destination, CancellationToken cancellationToken)
    => await graphServiceClient.MoveUserMessagesAsync(user, messages, destination.ToString(), cancellationToken);

  public async static Task<Message> MoveUserMessageToDeletedItemsAsync(this GraphServiceClient graphServiceClient, User user, Message message, CancellationToken cancellationToken)
    => await graphServiceClient.MoveUserMessageAsync(user, message, Constants.Url.DeletedItems, cancellationToken);

  public async static Task MoveUserMessagesToDeletedItemsAsync(this GraphServiceClient graphServiceClient, User user, IEnumerable<Message> messages, CancellationToken cancellationToken)
    => await graphServiceClient.MoveUserMessagesAsync(user, messages, Constants.Url.DeletedItems, cancellationToken);

  public static Users.Item.MailFolders.Item.MailFolderItemRequestBuilder GetUserMailFolderItemRequestBuilder(this GraphServiceClient graphServiceClient, User user, string mailFolderPath) {
    var userBuilder = graphServiceClient.Users[user.Id];
    var folderBuilder = userBuilder.MailFolders[mailFolderPath];
    return folderBuilder;
  }

  public static Users.Item.MailFolders.Item.MailFolderItemRequestBuilder GetUserMailFolderItemRequestBuilder(this GraphServiceClient graphServiceClient, User user, WellKnownFolder mailFolder)
    => graphServiceClient.GetUserMailFolderItemRequestBuilder(user, mailFolder.ToString());

  public static Users.Item.MailFolders.Item.Messages.MessagesRequestBuilder GetUserMessagesRequestBuilder(this GraphServiceClient graphServiceClient, User user, string mailFolderPath) 
    => graphServiceClient.GetUserMailFolderItemRequestBuilder(user, mailFolderPath).Messages;

  public static Users.Item.MailFolders.Item.Messages.MessagesRequestBuilder GetUserMessagesRequestBuilder(this GraphServiceClient graphServiceClient, User user, WellKnownFolder mailFolder)
    => graphServiceClient.GetUserMailFolderItemRequestBuilder(user, mailFolder).Messages;

  public async static Task<List<Message>> GetUserMessagesAsync(this GraphServiceClient graphServiceClient, User user, string mailFolderPath, Users.Item.MailFolders.Item.Messages.MessagesRequestBuilder.MessagesRequestBuilderGetQueryParameters queryParameters, CancellationToken cancellationToken) {
    var userBuilder = graphServiceClient.Users[user.Id];
    var folderBuilder = userBuilder.MailFolders[mailFolderPath];
    var messagesBuilder = folderBuilder.Messages;
    var inboxResponse = await messagesBuilder.GetAsync(requestConfiguration => {
      requestConfiguration.QueryParameters = queryParameters;
    }, cancellationToken: cancellationToken);
    return inboxResponse?.Value ?? new();
  }

  public async static Task<List<Message>> GetUserMessagesAsync(this GraphServiceClient graphServiceClient, User user, WellKnownFolder mailFolder, Users.Item.MailFolders.Item.Messages.MessagesRequestBuilder.MessagesRequestBuilderGetQueryParameters queryParameters, CancellationToken cancellationToken)
    => await graphServiceClient.GetUserMessagesAsync(user, mailFolder.ToString(), queryParameters, cancellationToken);

  public async static Task MoveUserMessagesToDeletedItemsAsync(this GraphServiceClient graphServiceClient, User user, WellKnownFolder mailFolder, string filter, CancellationToken cancellationToken) {
    var messages = await graphServiceClient.GetUserMessagesAsync(user, mailFolder, new Users.Item.MailFolders.Item.Messages.MessagesRequestBuilder.MessagesRequestBuilderGetQueryParameters {
      Filter = filter
    }, cancellationToken);
    foreach (var message in messages) {
      await graphServiceClient.MoveUserMessageToDeletedItemsAsync(user, message, cancellationToken);
    }
  }

  /// <summary>https://learn.microsoft.com/en-us/graph/api/message-update?view=graph-rest-1.0&tabs=csharp</summary>
  public async static Task<Message> UpdateUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, Message newMessageBody, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].PatchAsync(newMessageBody, cancellationToken: cancellationToken);

  public async static Task<Message> UpdateUserMessageAsync(this GraphServiceClient graphServiceClient, User user, Message message, Func<Message> newMessageBody, CancellationToken cancellationToken)
    => await graphServiceClient.Users[user.Id].Messages[message.Id].PatchAsync(newMessageBody(), cancellationToken: cancellationToken);

}