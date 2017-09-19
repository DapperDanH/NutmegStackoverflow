using System.Linq;
using System.Threading.Tasks;
using Nutmeg.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Nutmeg.Controllers
{
	public class UserProfileController : BaseApiController
	{
		#region Constructors
		public UserProfileController(NutmegContext dbContext)
			: base(dbContext)
		{
		}
		#endregion Constructors

		#region Methods
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Get)}()");

				var currentUserId = Thread.CurrentPrincipal.GetCurrentUserId();
				var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
				var dto = new UserProfileDto()
				{
					Id = user.Id,
					Email = user.EmailAddress
				};

				// determine which notifications they already subscribed to
				var subscriptions = await _dbContext.NotificationSubscription.Where(x => x.UserId == user.Id).ToListAsync();
				AddSubscriptionToDto(dto, NotificationType.NewDataAvailable, subscriptions);
				AddSubscriptionToDto(dto, NotificationType.NewPlaniswareProject, subscriptions);
				AddSubscriptionToDto(dto, NotificationType.NewMicrosoftProject, subscriptions);

				return Ok(dto);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Get)}()");
				throw;
			}
		}

		private void AddSubscriptionToDto(UserProfileDto dto, NotificationType notificationType, List<NotificationSubscription> subscriptions)
		{
			var foundItem = subscriptions.FirstOrDefault(x => x.Type == (short)notificationType);

			var notify = NotifyType.None;
			if (foundItem != null)
			{
				if (foundItem.IncludeApp && foundItem.IncludeEmail)
				{
					notify = NotifyType.AppAndEmail;
				}
				else if (foundItem.IncludeApp && !foundItem.IncludeEmail)
				{
					notify = NotifyType.InAppOnly;
				}
			}

			dto.Subscriptions.Add(new NotificationSubscriptionDto()
			{
				Type = notificationType,
				Notify = notify
			});
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody]UserProfileDto dto)
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Put)}({nameof(dto)} = {JsonConvert.SerializeObject(dto)})");

				var currentUserId = Thread.CurrentPrincipal.GetCurrentUserId();
				var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
				if (user == null)
				{
					_logger.Warn(() => $"Entity not found with id: {dto.Id}");
					return NotFound();
				}

				// update the email address
				user.EmailAddress = dto.Email;

				// add/update/remove any of the notifications subscriptions
				var subscriptions = await _dbContext.NotificationSubscription.Where(x => x.UserId == user.Id).ToListAsync();
				foreach (var dtoSubscription in dto.Subscriptions)
				{
					var foundItem = subscriptions.FirstOrDefault(x => x.Type == (short)dtoSubscription.Type);
					if (dtoSubscription.Notify != NotifyType.None && foundItem == null)
					{
						var subscription = new NotificationSubscription()
						{
							Id = Guid.NewGuid(),
							UserId = user.Id,
							Type = (short)dtoSubscription.Type,
							IncludeApp = true,
							IncludeEmail = (dtoSubscription.Notify == NotifyType.AppAndEmail)
						};
						_dbContext.NotificationSubscription.Add(subscription);
					}
					else if (foundItem != null)
					{
						foundItem.IncludeApp = (dtoSubscription.Notify != NotifyType.None);
						foundItem.IncludeEmail = (dtoSubscription.Notify == NotifyType.AppAndEmail);
					}
				}

				await _dbContext.SaveChangesAsync();
				return Ok(dto);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Put)}");
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
		#endregion Methods
	}
}
