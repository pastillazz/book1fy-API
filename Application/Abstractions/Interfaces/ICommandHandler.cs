using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Interfaces;

public interface ICommandHandler<TCommand>:IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> :IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ }