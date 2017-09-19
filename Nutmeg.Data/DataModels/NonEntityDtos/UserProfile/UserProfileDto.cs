using System;
using System.Collections.Generic;

namespace Nutmeg.Data
{
	public class UserProfileDto : KeyedDto<Guid>
	{
		public string Email { get; set; }
		public List<NotificationSubscriptionDto> Subscriptions { get; protected set; } = new List<NotificationSubscriptionDto>();
	}
}
