using Domain.Primitives;

namespace Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> 
            inputQuery, Specification<TEntity> specification) 
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQuery;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }
        
        queryable=specification.IncludeExpressions.Aggregate(
            queryable, (current, includeExpression)=>
                includeExpression(current));

        if (specification.OrderByExpression is not null)
        {
            queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        return queryable;
    }
}