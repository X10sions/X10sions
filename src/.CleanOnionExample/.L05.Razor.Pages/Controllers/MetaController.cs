using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace CleanOnionExample.Controllers;

public class MetaController : BaseApiController {
  [HttpGet("/info")]
  public ActionResult<string> Info() {
    var message = $"Current: {GetInfo(typeof(MetaController).Assembly)}" +
      $"Calling: {GetInfo(Assembly.GetCallingAssembly())}" +
      $"Executing: {GetInfo(Assembly.GetExecutingAssembly())}" +
      $"Entry: {GetInfo(Assembly.GetEntryAssembly())}";
    return Ok(message);
  }

  public string GetInfo(Assembly assembly) {
    return $"Assembly: {assembly.FullName} " +
      $"Version: {FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion}, " +
      $"Created: {System.IO.File.GetCreationTime(assembly.Location)}, " +
      $"Updated: {System.IO.File.GetLastWriteTime(assembly.Location)}";
  }

}