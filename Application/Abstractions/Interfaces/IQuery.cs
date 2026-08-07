using Domain.Abstractions;
using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }