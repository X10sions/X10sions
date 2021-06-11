using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDB.Driver {
  public static class IMongoDatabaseExtensions {

    public static T InsertOne<T>(this IMongoDatabase db, string collectionName, T entity) {
      db.GetCollection<T>(collectionName).InsertOne(entity);
      return entity;
    }

    public static async Task<T> InsertOneAsync<T>(this IMongoDatabase db, string collectionName, T entity) {
      await db.GetCollection<T>(collectionName).InsertOneAsync(entity);
      return entity;
    }

    public static IEnumerable<T> InsertMany<T>(this IMongoDatabase db, string collectionName, IEnumerable<T> entities) {
      db.GetCollection<T>(collectionName).InsertMany(entities);
      return entities;
    }

    public static IEnumerable<T> ToList<T>(this IMongoDatabase db, string collectionName,
      Expression<Func<T, bool>> filter = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> sortBy = null,
      int? limit = null,
      int? skip = null
      ) {
      //Expression<Func<Shoe, bool>> testFilter = x => x.Price > 2 && x.Gender == Gender.Female;
      //Expression<Func<Shoe, object>> testSort = x => new { x.Price, x.Name };
      //Expression<Func<Shoe, object>> testProjection = x => new { x.Id, x.Name, x.Price };
      //var test2 = Database.ToList(nameof(Shoes), testFilter, testSort, testProjection, 5, 2);

      IQueryable<T> qry = db.GetCollection<T>(collectionName).AsQueryable();
      if (filter != null) { qry = qry.Where(filter); }
      if (limit.HasValue) { qry = qry.Take(limit.Value); }
      if (skip.HasValue) { qry = qry.Skip(skip.Value); }
      if (sortBy != null) { qry = sortBy(qry); }
      return qry.ToList();
      //return db.GetCollection<T>(collectionName).Find(filter).Limit(limit).Skip(skip).Sort().Project(projection).ToList();
    }

    public static IEnumerable<TNewProjection> ToList<T, TNewProjection>(this IMongoDatabase db, string collectionName,
      Expression<Func<T, TNewProjection>> projection,
      Expression<Func<T, bool>> filter = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> sortBy = null,
      int? limit = null,
      int? skip = null
      ) {
      //Expression<Func<Shoe, bool>> testFilter = x => x.Price > 2 && x.Gender == Gender.Female;
      //Expression<Func<Shoe, object>> testSort = x => new { x.Price, x.Name };
      //Expression<Func<Shoe, object>> testProjection = x => new { x.Id, x.Name, x.Price };
      //var test2 = Database.ToList(nameof(Shoes), testFilter, testSort, testProjection, 5, 2);
      IQueryable<T> qry = db.GetCollection<T>(collectionName).AsQueryable();
      if (filter != null) { qry = qry.Where(filter); }
      if (limit.HasValue) { qry = qry.Take(limit.Value); }
      if (skip.HasValue) { qry = qry.Skip(skip.Value); }
      if (sortBy != null) { qry = sortBy(qry); }
      return qry.Select(projection).ToList();
      //return db.GetCollection<T>(collectionName).Find(filter).Limit(limit).Skip(skip).Sort().Project(projection).ToList();
    }


    //public static IEnumerable<T> GetList<T>(this IMongoDatabase db, string collectionName, IEnumerable<T> entities) {
    //  db.GetCollection<T>(collectionName).InsertMany(entities);
    //  var filterBuilder = Builders<T>.Filter;
    //  var projectionBuilder = Builders<T>.Projection;
    //  var collection = db.GetCollection<T>(collectionName);
    //  //var filter = filterBuilder.ElemMatch("carpenter..., ?"); //<--- ???
    //  var projection = projectionBuilder.Exclude("_id");
    //  var list = await collection.Find(filter).Project(projection).ToListAsync();
    //  return list;
    //}

    public static IEnumerable<BsonValue> GetCollectionsWithName(this IMongoDatabase db) {
      var bre = new BsonRegularExpression("<YOUR REGEX PATTERN>");
      var copt = new ListCollectionsOptions {
        Filter = Builders<BsonDocument>.Filter.Regex("name", bre)
      };
      return db.ListCollections(copt).ToList().Select(col => col["name"]);
    }

    //public static string  GetCollectionName<T>(this IMongoDatabase db) {
    //  var customers = db.GetCollection<T>(typeof(T).Name.Pluralize().ToLower());

    //}
    

  }
}
