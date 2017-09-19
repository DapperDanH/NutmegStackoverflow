using Nutmeg.Data;
using System;

namespace Nutmeg.Data
{


	public class UserDto : DtoForEntity<User, Guid>
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string EmailAddress { get; set; }
		public bool Active { get; set; }
	}
}
