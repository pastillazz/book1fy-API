using Application.Common.Abstractions.Interfaces;
using Domain.Shared;

namespace Application.Users.Commands.RevokeRefreshTokens;

public record RevokeRefreshTokensCommand() : ICommand;
