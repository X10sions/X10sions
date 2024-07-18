using Common.Data;
using X10sions.Fake.Features.Project;
using X10sions.Fake.Features.ToDo;
using X10sions.Fake.Features.ToDo.Item;
using X10sions.Fake.Features.WeatherForecast;

namespace CleanOnionExample.Data.DbContexts;

public static class ICleanOnionExampleErpDatabaseExtensions {
  public static IDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase> ToDoItem(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<ToDoList, CleanOnionExampleErpDatabase> ToDoList(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<ToDoList, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<Project, CleanOnionExampleErpDatabase> Project(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<Project, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<WeatherForecast, CleanOnionExampleErpDatabase> WeatherForecast(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<WeatherForecast, CleanOnionExampleErpDatabase>();
}

/*

add-migration v1

update-database


 */