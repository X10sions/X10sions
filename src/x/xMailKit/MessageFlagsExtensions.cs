namespace MailKit;

public static class MessageFlagsExtensions {
  public static bool IsDeleted(this MessageFlags? flags) => flags.Value.HasFlag(MessageFlags.Deleted);
  public static bool IsDraft(this MessageFlags? flags) => flags.Value.HasFlag(MessageFlags.Draft);
  public static bool IsFlagged(this MessageFlags? flags) => flags.Value.HasFlag(MessageFlags.Flagged);
  public static bool IsSeen(this MessageFlags? flags) => flags.Value.HasFlag(MessageFlags.Seen);
}