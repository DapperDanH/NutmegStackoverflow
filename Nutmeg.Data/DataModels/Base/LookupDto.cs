using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Provides functionality for a data transfer object with a unique ID and name of a particular type.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity that the data transfer object corresponds to.</typeparam>
	/// <typeparam name="TKey">The type of the ID property.</typeparam>
	public class LookupDto<TEntity, TKey> : DtoForEntity<TEntity, TKey>
		where TEntity : IKeyed<TKey>
	{
		/// <summary>
		/// Gets or sets the name of the data transfer object.
		/// </summary>
		public virtual string Name { get; set; }
	}
}
