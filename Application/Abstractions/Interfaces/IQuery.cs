using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }