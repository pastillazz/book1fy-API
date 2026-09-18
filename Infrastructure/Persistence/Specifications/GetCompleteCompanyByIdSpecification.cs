using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Specifications;

public class GetCompleteCompanyByIdSpecification:Specification<Company>
{
    public GetCompleteCompanyByIdSpecification(Guid companyId, Guid serviceId) :
        base(c => c.Id == companyId)
    {
      AddInclude(q=>q
          .Include(c =>
              c.Services.Where(s => s.Id == serviceId))
          .ThenInclude(s => s.Tickets));
    }
    
   

}
