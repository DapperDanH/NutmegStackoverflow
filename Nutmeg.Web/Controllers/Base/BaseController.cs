using Microsoft.AspNetCore.Mvc;
using NLog;
using Nutmeg.Data;

namespace Nutmeg.Controllers
{
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public abstract class BaseController : Controller
	{
		#region Fields/Properties
		protected readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Gets the Entity Framework context for the entities in the system.
		/// </summary>
		protected readonly NutmegContext _dbContext;
		#endregion Fields/Properties

		#region Constructors
		protected BaseController(NutmegContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion Constructors

		#region Methods
		protected override void Dispose(bool disposing)
		{
			_dbContext?.Dispose();
			base.Dispose(disposing);
		}
		#endregion Methods
	}
}
