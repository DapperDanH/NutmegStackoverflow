using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates an entity that wants to be notified when it's being added.
	/// </summary>
	/// <typeparam name="TContext">The type of the context with which the entity is added.</typeparam>
	public interface INotifyAdded<TContext>
	{
		/// <summary>
		/// Called when the entity is being added.
		/// </summary>
		/// <param name="context">The context with which the entity is being added.</param>
		void OnAdded(TContext context);
	}
}
