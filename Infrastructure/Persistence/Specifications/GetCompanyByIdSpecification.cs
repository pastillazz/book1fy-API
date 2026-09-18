using Domain.Entities;

namespace Infrastructure.Persistence.Specifications;

public class GetCompanyByIdSpecification : Specification<Company>
{
    public GetCompanyByIdSpecification(Guid companyId ) : base(c=> c.Id == companyId){}
     
}
