using Common.Features.DummyFakeExamples.ToDo.Item;
using MediatR;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemService : IToDoItemService {
  private readonly IToDoItemRepository _repository;
  private readonly IToDoItemFactory _factory;
  private readonly ToDoItemViewModelMapper _viewModelMapper;
  private readonly IMediator _mediator;

  public ToDoItemService(IToDoItemRepository repository, ToDoItemViewModelMapper viewModelMapper, IToDoItemFactory factory, IMediator mediator) {
    _repository = repository;
    _viewModelMapper = viewModelMapper;
    _factory = factory;
    _mediator = mediator;
  }

  public async Task<ToDoItemViewModel> Create(ToDoItemViewModel viewModel) {
    var newCommand = _viewModelMapper.ConvertToNewToDoItemCommand(viewModel);
    var result = await _mediator.Send((IRequest<ToDoItem>)newCommand);
    return _viewModelMapper.ConstructFromEntity(result);
  }

  public async Task Delete(Guid id) {
    var deleteCommand = _viewModelMapper.ConvertToDeleteToDoItemCommand(id);
    await _mediator.Publish(deleteCommand);
  }

  public async Task<IEnumerable<ToDoItemViewModel>> GetAll() {
    var entities = await _repository.GetAllAsync();
    return _viewModelMapper.ConstructFromListOfEntities(entities);
  }

  public async Task<ToDoItemViewModel> GetById(Guid id) {
    var entity = await _repository.GetByIdAsync(new ToDoItemId(id));
    return _viewModelMapper.ConstructFromEntity(entity);
  }
}