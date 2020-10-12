using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Commands.GenericCommand;
using Common.Application.Queries.GenericQueries;
using Common.Domain.Entities;
using Infrastructure.ApplicationFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BizOS.Accounts.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [GenericControllerNameConvention(typeof(GenericController<>))]
  public class GenericController<T> : ControllerBase where T : Entity
  {
    Guid accountId = Guid.NewGuid();
    private readonly IMediator mediator;

    public GenericController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpGet]
    // URL : api/controller
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetEntities<T>(), cancellationToken));
    }

    // URL : api/controller/searchQuery 
    [HttpGet("search/{query}")]
    public async Task<IActionResult> Get(string query, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetEntities<T>() { SearchTerm = query }, cancellationToken));
    }

    // URL : api/controller/id
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(new GetEntity<T>(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> InsertAsync([FromBody] T obj, CancellationToken cancellationToken)
    {
      var createRequest = new CreateEntityCommand<T>(accountId, obj);
      return this.Ok(await mediator.Send(createRequest, cancellationToken));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      var deleteRequest = new DeleteEntityCommand<T>(accountId, id);
      return this.Ok(await mediator.Send(deleteRequest, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(T obj, CancellationToken cancellationToken)
    {
      var updateRequest = new UpdateEntityCommand<T>(accountId, obj.Id, obj);
      return this.Ok(await mediator.Send(updateRequest, cancellationToken));
    }
  }
}
