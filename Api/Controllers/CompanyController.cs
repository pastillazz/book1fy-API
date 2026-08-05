using Api.Abstractions;
using MediatR;

namespace Api.Controllers;

public class CompanyController(ISender sender) : ApiController(sender);