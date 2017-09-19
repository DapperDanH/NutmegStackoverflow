using Nutmeg.Data.Assets;
using System;

namespace Nutmeg.Data
{
	public class EntityValidationException : Exception
	{
		#region Constructors
		public EntityValidationException(string message)
			: base(message)
		{
		}
		#endregion Constructors

		#region Methods
		public static void GenericError(string message)
		{
			throw new EntityValidationException(message);
		}
		public static void ForeignKeyError(string targetType, string foreignType)
		{
			var message = string.Format(AppStrings.ForeignKeyError, targetType, foreignType);
			throw new EntityValidationException(message);
		}
		public static void ForeignKeyError(string targetType, string primaryForeignType, string secondaryForeignType)
		{
			var message = string.Format(AppStrings.ForeignKeySecondaryError, targetType, primaryForeignType, secondaryForeignType);
			throw new EntityValidationException(message);
		}
		public static void ForeignKeyErrorPlural(string targetType, string foreignType)
		{
			var message = string.Format(AppStrings.ForeignKeyErrorPlural, targetType, foreignType);
			throw new EntityValidationException(message);
		}
		public static void ForeignKeyErrorPlural(string targetType, string primaryForeignType, string secondaryForeignType)
		{
			var message = string.Format(AppStrings.ForeignKeySecondaryErrorPlural, targetType, primaryForeignType, secondaryForeignType);
			throw new EntityValidationException(message);
		}
		#endregion Methods
	}
}
