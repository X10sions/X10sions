using Common.Data;
using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanOnionExample.Controllers;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class ProjectsController : BaseApiController {
  public ProjectsController(IRepository<Project> repository) {
    this.repository = repository;
  }

  private readonly IRepository<Project> repository;

  // GET: api/Projects
  [HttpGet]
  public async Task<IActionResult> List() {
    var projectDTOs = await repository.Query.Select(x => new ProjectDTO(x.Id, x.Name)).ToListAsync(); ;
    return Ok(projectDTOs);
  }

  // GET: api/Projects
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id) {
    var project = await repository.GetByIdAsync(id);
    if (project == null) {
      return NotFound();    }
    var result = new ProjectDTO(project.Id, project.Name, new List<ToDoItemDTO>(project.Items.Select(i => new ToDoItemDTO(i)).ToList()));
    return Ok(result);
  }

  // POST: api/Projects
  [HttpPost]
  public async Task<IActionResult> Post([FromBody] CreateProjectDTO request) {
    var newProject = new Project(request.Name, PriorityStatus.Backlog);
    var newId = await repository.InsertWithIdAsync<int>(newProject);
    var result = new ProjectDTO(newId, newProject.Name);
    return Ok(result);
  }

  // PATCH: api/Projects/{projectId}/complete/{itemId}
  [HttpPatch("{projectId:int}/complete/{itemId}")]
  public async Task<IActionResult> Complete(int projectId, int itemId) {
    var project = await repository.GetByIdAsync(projectId);
    if (project == null) return NotFound("No such project");
    var toDoItem = project.Items.FirstOrDefault(item => item.Id == itemId);
    if (toDoItem == null) return NotFound("No such item.");
    toDoItem.MarkComplete();
    await repository.UpdateAsync(project);
    return Ok();
  }
}
