using Domain.Abstractions;
using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Interfaces;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }