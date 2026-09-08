namespace Application.Common.Abstractions.Link;

public record Link(
    string Href,
    string Rel,
    string Method);