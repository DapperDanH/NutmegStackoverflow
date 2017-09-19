using Nutmeg.Data;
using System;

namespace Nutmeg.Controllers
{
	//[AuthorizeRoles(Roles.Admin)]
	public class UserController : RestfulApiController<User, UserDto, Guid>
	{
		#region Constructors
		public UserController(NutmegContext dbContext)
			: base(dbContext)
		{
		}
		#endregion Constructors
	}
}
