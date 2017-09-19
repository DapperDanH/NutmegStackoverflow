using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates an entity with a unique ID of a particular type.
	/// </summary>
	/// <typeparam name="TKey">The type of the ID property.</typeparam>
	public interface IKeyed<TKey>
	{
		/// <summary>
		/// Gets or sets the ID of the entity.
		/// </summary>
		TKey Id { get; set; }
	}
}
