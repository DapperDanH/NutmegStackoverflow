using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates a data transfer object with a unique ID of a particular type.
	/// </summary>
	/// <typeparam name="TKey">The type of the ID property.</typeparam>
	public interface IKeyedDto<TKey>
	{
		/// <summary>
		/// Gets or sets the ID of the data transfer object.
		/// </summary>
		TKey Id { get; set; }
	}

	/// <summary>
	/// Provides base functionality for a data transfer object with a unique ID of a particular type.
	/// </summary>
	/// <typeparam name="TKey">The type of the ID property.</typeparam>
	public abstract class KeyedDto<TKey> : IKeyedDto<TKey>
	{
		/// <summary>
		/// Gets or sets the ID of the data transfer object.
		/// </summary>
		public virtual TKey Id { get; set; }
	}
}
