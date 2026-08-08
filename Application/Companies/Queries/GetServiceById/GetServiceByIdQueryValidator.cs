using FluentValidation;

namespace Application.Companies.Queries.GetServiceById;

public class GetServiceByIdQueryValidator : AbstractValidator<GetServiceByIdQuery>
{
    public GetServiceByIdQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();
    }
}
