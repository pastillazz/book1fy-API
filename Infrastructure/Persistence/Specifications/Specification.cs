using System.Linq.Expressions;
using Domain.Primitives;

namespace Infrastructure.Persistence.Specifications;

public abstract class Specification<TEntity> where
TEntity :Entity
{
  protected Specification(Expression<Func<TEntity, bool>> criteria)=>
    Criteria=criteria;

  public Expression<Func<TEntity, bool>>? Criteria { get; }
  
  //public List<Expression<Func<TEntity, object>>> IncludeExpressions{get;}=new();
  public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeExpressions { get; } = new();
  public Expression<Func<TEntity, object>>? OrderByExpression { get; private set;}
 
  public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

  protected void AddInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> 
    includeExpression)=>
    IncludeExpressions.Add(includeExpression);
   
  protected void AddOrderBy (Expression<Func<TEntity, object>> orderByExpression)=>
    OrderByExpression=orderByExpression;
  
  protected void AddOrderByDescending(Expression<Func<TEntity,object>> orderByDescendingExpression)=>
    OrderByDescendingExpression = orderByDescendingExpression;


}

