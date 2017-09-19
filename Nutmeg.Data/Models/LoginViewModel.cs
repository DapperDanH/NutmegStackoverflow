using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nutmeg.Data
{
	public class LoginViewModel2
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Keep me logged in")]
		public bool KeepMeLoggedIn { get; set; }
	}
}
