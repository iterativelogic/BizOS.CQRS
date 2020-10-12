using Microsoft.AspNetCore.Mvc;

namespace BizOS.Accounts.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  public class VoucherController : ControllerBase
  {
  }
}
