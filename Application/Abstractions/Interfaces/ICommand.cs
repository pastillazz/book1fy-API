using Domain.Abstractions;
using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Interfaces;

public interface ICommand:IRequest<Result>
{ }

public interface ICommand<TResponse>:IRequest<Result<TResponse>>
{ }