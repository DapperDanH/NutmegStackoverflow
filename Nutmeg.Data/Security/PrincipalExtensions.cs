using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;

namespace Nutmeg.Data
{
	public static class PrincipalExtensions
	{
		#region Methods
		public static Guid GetCurrentUserId(this IPrincipal principal, bool systemAccountIfNotFound = true)
		{
			var claimsPrincipal = principal as ClaimsPrincipal;
			if (claimsPrincipal != null)
			{
				var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == Claims.UserID);
				if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
				{
					return Guid.Parse(userIdClaim.Value);
				}
			}

			if (systemAccountIfNotFound)
			{
				return Constants.SystemUserId;
			}

			return Guid.Empty;
		}
		public static string GetCurrentUserName(this IPrincipal principal)
		{
			var claimsPrincipal = principal as ClaimsPrincipal;
			if (claimsPrincipal != null)
			{
				var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == Claims.DisplayName);
				if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
				{
					return userIdClaim.Value;
				}
			}

			return "Unknown";
		}
		#endregion Methods
	}
}
