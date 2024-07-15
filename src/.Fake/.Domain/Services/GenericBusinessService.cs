using Common.Collections.Paged;
using Common.Domain;
using Common.Domain.Entities;
using Common.Domain.Exceptions;
using Common.Domain.Repositories;
using Common.Results;
using System.Linq.Expressions;

namespace X10sions.Fake.Domain.Services;

public class GenericBusinessService<T, TKey> : IGenericBusinessService<T, TKey> where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  protected readonly IUnitOfWork _unitOfWork;
  protected readonly IBusinessRepository<T, TKey> _repository;
  public GenericBusinessService(IUnitOfWork unitOfWork) {
    _unitOfWork = unitOfWork;
    _repository = _unitOfWork.GetBusinessRepository<T, TKey>();
  }

  public List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetAll(navigationPropertiesToLoad);

  public List<T> GetAllFiltered(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetAllFiltered(predicate, navigationPropertiesToLoad);

  public IPagedList<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy = "Id", int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetAllFilteredPaged(predicate, orderBy, startRowIndex, maxRows, navigationPropertiesToLoad);

  public IPagedList<T> GetAllPaged(string orderBy = "Id", int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetAllPaged(orderBy, startRowIndex, maxRows, navigationPropertiesToLoad);

  public T GetById(TKey id, params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetById(id);

  public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad) => _repository.GetSingle(predicate, navigationPropertiesToLoad);

  //Note that inside Add()/Delete()/Update() methods DataNotUpdatedException is an Error whereas BusinessException is a Warning

  public IBusinessResult Add(T entity) {
    IBusinessResult businessResult;
    try {
      OnAdding(entity);
      _repository.Add(entity);
      if (_unitOfWork.SaveChanges() == 0) {
        throw new DataNotUpdatedException("Operation failed!");
      }
      OnAdded(entity);
      businessResult = BusinessResult.Success;
    } catch (DataNotUpdatedException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Error });
    } catch (BusinessException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Warning });
    } catch (Exception E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = "Error!", MessageType = MessageResult.Type.Error });
    }
    return businessResult;
  }

  public IBusinessResult Delete(T entity) {
    IBusinessResult businessResult;
    try {
      OnDeleting(entity);
      _repository.Delete(entity);
      if (_unitOfWork.SaveChanges() == 0) {
        throw new DataNotUpdatedException("Operation failed!");
      }
      businessResult = BusinessResult.Success;
      OnDeleted(entity);
    } catch (DataNotUpdatedException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Error });
    } catch (BusinessException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Warning });
    } catch (Exception E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = "Error!", MessageType = MessageResult.Type.Error });
    }
    return businessResult;
  }

  public IBusinessResult Update(T entity) {
    IBusinessResult businessResult;
    try {
      OnDeleting(entity);
      _repository.Update(entity);
      if (_unitOfWork.SaveChanges() == 0) {
        throw new DataNotUpdatedException("Operation failed!");
      }
      businessResult = BusinessResult.Success;
      OnDeleted(entity);
    } catch (DataNotUpdatedException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Error });
    } catch (BusinessException E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = E.Message, MessageType = MessageResult.Type.Warning });
    } catch (Exception E) {
      businessResult = new BusinessResult();
      businessResult.Messages.Add(new MessageResult { Message = "Error", MessageType = MessageResult.Type.Error });
    }
    return businessResult;
  }

  public virtual void OnAdding(T entity) { }
  public virtual void OnAdded(T entity) { }
  public virtual void OnUpdating(T entity) { }
  public virtual void OnUpdated(T entity) { }
  public virtual void OnDeleting(T entity) { }
  public virtual void OnDeleted(T entity) { }

  public virtual Expression<Func<T, object>>[] GetDefaultLoadProperties() => Array.Empty<Expression<Func<T, object>>>();
}