using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Commands.GenericTenantCommand;
using Common.Application.Queries.GenericTenantQueries;
using Common.Domain.Entities;
using Infrastructure.ApplicationFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BizOS.Accounts.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  [ApiController]
  [GenericControllerNameConvention(typeof(GenericTenantController<>))]
  public class GenericTenantController<T> : ControllerBase where T: TenantEntity
  {
    Guid accountId = Guid.NewGuid();
    Guid tenantId = Guid.NewGuid();
    private readonly IMediator mediator;

    public GenericTenantController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpGet]
    // URL : api/controller
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetTenantEntities<T>(tenantId), cancellationToken));
    }

    // URL : api/controller/searchQuery 
    [HttpGet("search/{query}")]
    public async Task<IActionResult> Get(string query, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetTenantEntities<T>(tenantId) { SearchTerm = query }, cancellationToken));
    }

    // URL : api/controller/id
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetTenantEntity<T>(tenantId, id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> InsertAsync([FromBody] T obj, CancellationToken cancellationToken)
    {
      var createRequest = new CreateTenantEntityCommand<T>(tenantId, accountId, obj);
      return this.Ok(await mediator.Send(createRequest, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] Guid id, CancellationToken cancellationToken)
    {
      var deleteRequest = new DeleteTenantEntityCommand<T>(tenantId, accountId, id);
      return this.Ok(await mediator.Send(deleteRequest, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(T obj, CancellationToken cancellationToken)
    {
      var updateRequest = new UpdateTenantEntityCommand<T>(tenantId, accountId, obj.Id, obj);
      return this.Ok(await mediator.Send(updateRequest, cancellationToken));
    }
  }
}
