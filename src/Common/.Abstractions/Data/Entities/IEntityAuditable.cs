namespace Common.Data.Entities;

public interface IEntityAuditable {
  DateTime? DeleteDate { get; set; }
  string? DeleteUserName { get; set; }
  DateTime InsertDate { get; set; }
  string InsertUserName { get; set; }
  DateTime? UpdateDate { get; set; }
  string? UpdateUserName { get; set; }

}

//public interface IEntityAuditable<TKey> : IEntityWithId<TKey> where TKey : IEquatable<TKey> {
//  string CreatedBy { get; set; }
//  DateTime CreatedOn { get; set; }
//  string LastModifiedBy { get; set; }
//  DateTime? LastModifiedOn { get; set; }

//}