using Common.Data;
using X10sions.Fake.Features.ToDo.Item;
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using NHibernate;

namespace CleanOnionExample.Data.DbContexts;

public class CleanOnionExampleErpDatabase : IDatabase, IHaveDataContext, IHaveDbContext, IHaveSession {
  public CleanOnionExampleErpDatabase(DbContext dbContext, DataContext dataContext, ISession session) {
    DbContext = dbContext;
    DataContext = dataContext;
    Session = session;
  }
  public DbContext DbContext { get; }
  public DataContext DataContext { get; }
  public ISession Session { get; }

  public IDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase> ToDoItem => new DatabaseTable<ToDoItem, CleanOnionExampleErpDatabase>(this);
  //public IDatabaseTable<ToDoList, ICleanOnionExampleErpDatabase> ToDoList => new DatabaseTable<ToDoList>(this);
  //public IDatabaseTable<Project, ICleanOnionExampleErpDatabase> Project => new DatabaseTable<Project>(this);
  //public IDatabaseTable<WeatherForecast, ICleanOnionExampleErpDatabase> WeatherForecast => new CleanOnionExampleErpDatabaseTable<WeatherForecast>(this);
}
