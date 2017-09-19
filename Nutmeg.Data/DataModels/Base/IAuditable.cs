using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates an entity on which auditing information is being tracked using <see cref="DateTimeOffset"/>.
	/// </summary>
	public interface IAuditable
	{
		/// <summary>
		/// Gets or sets the ID of the user who created the entity.
		/// </summary>
		Guid CreatedById { get; set; }

		/// <summary>
		/// Gets or sets the date on which the entity was created.
		/// </summary>
		DateTimeOffset CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the ID of the user who last modified the entity.
		/// </summary>
		Guid LastModifiedById { get; set; }

		/// <summary>
		/// Gets or sets the date on which the entity was last modified.
		/// </summary>
		DateTimeOffset LastModifiedOn { get; set; }
	}
}
