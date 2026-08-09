using Domain.Shared;
using MediatR;

namespace Application.Common.Abstractions.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }