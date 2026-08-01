using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CompanyController:ApiController
{
    protected CompanyController(ISender sender) : base(sender){}

  
}