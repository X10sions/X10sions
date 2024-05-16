using System.Runtime.Serialization;

namespace Microsoft.Graph.Models;

//https://learn.microsoft.com/en-us/graph/api/resources/mailfolder?view=graph-rest-1.0
public enum WellKnownFolder {
  [EnumMember(Value = "archive")] Archive,
  [EnumMember(Value = "clutter")] Clutter,
  [EnumMember(Value = "conflicts")] Conflicts,
  [EnumMember(Value = "conversationHistory")] ConversationHistory,
  [EnumMember(Value = "deletedItems")] DeletedItems,
  [EnumMember(Value = "drafts")] Drafts,
  [EnumMember(Value = "inbox")] Inbox,
  [EnumMember(Value = "junkEmail")] JunkEmail,
  [EnumMember(Value = "localFailures")] LocalFailures,
  [EnumMember(Value = "msgFolderRoot")] MsgFolderRoot,
  [EnumMember(Value = "outbox")] Outbox,
  [EnumMember(Value = "recoverableItemsDeletions")] RecoverableItemsDeletions,
  [EnumMember(Value = "scheduled")] Scheduled,
  [EnumMember(Value = "searchFolders")] SearchFolders,
  [EnumMember(Value = "sentItems")] SentItems,
  [EnumMember(Value = "serverFailures")] ServerFailures,
  [EnumMember(Value = "syncIssues")] SyncIssues,
}