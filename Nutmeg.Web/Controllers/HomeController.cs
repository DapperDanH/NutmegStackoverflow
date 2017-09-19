using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nutmeg.Data;
using Microsoft.EntityFrameworkCore;

namespace Nutmeg.Controllers
{
	//[Authorize]
	//[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class HomeController : BaseController
    {
		#region Constructors
		public HomeController(NutmegContext dbContext)
			: base(dbContext)
		{
		}
		#endregion Constructors

		#region Methods
		public async Task<IActionResult> Index()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Index)}()");
				throw;
			}
		}

		[AllowAnonymous]
		public IActionResult Error(int id)
		{
			if (id == (int)HttpStatusCode.NotFound)
			{
				var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
				if (statusFeature != null)
				{
					_logger.Warn($"Web Error: {id}. Path={statusFeature.OriginalPath}");
				}
			}
			else
			{
				_logger.Warn($"Web Error: {id}");
			}

			return View(id);
		}
		#endregion Methods
	}
}
