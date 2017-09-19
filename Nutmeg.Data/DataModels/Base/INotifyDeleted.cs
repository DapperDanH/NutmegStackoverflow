using System;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates an entity that wants to be notified when it's being deleted.
	/// </summary>
	/// <typeparam name="TContext">The type of the context with which the entity is deleted.</typeparam>
	public interface INotifyDeleted<TContext>
	{
		/// <summary>
		/// Called when the entity is being deleted.
		/// </summary>
		/// <param name="context">The context with which the entity is being deleted.</param>
		void OnDeleted(TContext context);
	}
}
