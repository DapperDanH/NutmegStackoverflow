using Microsoft.AspNetCore.Mvc;
using Nutmeg.Data;

namespace Nutmeg.Controllers
{
	//[Authorize]
	[Route("api/[controller]")]
	public abstract class BaseApiController : BaseController
	{
		#region Constructors
		protected BaseApiController(NutmegContext dbContext)
			: base(dbContext)
		{
		}
		#endregion Constructors
	}
}
