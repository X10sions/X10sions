using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.Messages.Item.Move;

namespace Microsoft.Graph.Users.Item.Messages;

public static class MessagesRequestBuilderExtensions {

  public async static Task<Message> MoveMessageAsync(this MessagesRequestBuilder builder, Message message, string destinationFolderId, CancellationToken cancellationToken)
    => await builder[message.Id].Move.PostAsync(new MovePostRequestBody {
      DestinationId = destinationFolderId
    }, cancellationToken: cancellationToken);

  public async static Task<Message> MoveMessageAsync(this MessagesRequestBuilder builder, Message message, WellKnownFolder destination, CancellationToken cancellationToken)
    => await builder.MoveMessageAsync(message, destination.ToString(), cancellationToken);

}