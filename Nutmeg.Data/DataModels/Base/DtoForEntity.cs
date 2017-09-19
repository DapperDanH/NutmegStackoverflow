using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nutmeg.Data
{
	/// <summary>
	/// Indicates a data transfer object that corresponds to a particular entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity that the data transfer object corresponds to.</typeparam>
	public interface IDtoForEntity<TEntity, TKey>
	{
		/// <summary>
		/// Gets or sets the primary key of the data transfer object.
		/// </summary>
		TKey Id { get; set; }

		/// <summary>
		/// Copies the value of all entity properties from the entity to the data transfer object.
		/// </summary>
		/// <param name="entity">The entity from which to the pull the property values.</param>
		/// <param name="propertiesToExclude">The names of any properties to exclude from the copying.</param>
		void CopyFrom(TEntity entity, params string[] propertiesToExclude);

		/// <summary>
		/// Copies the value of all entity properties from the data transfer object to the entity.
		/// </summary>
		/// <param name="entity">The entity to which to the push the property values.</param>
		/// <param name="propertiesToExclude">The names of any properties to exclude from the copying.</param>
		void CopyTo(TEntity entity, params string[] propertiesToExclude);
	}

	/// <summary>
	/// Provides base functionality for a data transfer object that corresponds to a particular entity.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity that the data transfer object corresponds to.</typeparam>
	/// <typeparam name="TKey">The type of the ID property.</typeparam>
	public abstract class DtoForEntity<TEntity, TKey> : KeyedDto<TKey>, IDtoForEntity<TEntity, TKey>
		where TEntity : IKeyed<TKey>
	{
		/// <summary>
		/// Copies the value of all entity properties from the entity to the data transfer object.
		/// </summary>
		/// <param name="entity">The entity to copy the property values from.</param>
		/// <param name="propertiesToExclude">The names of any properties to exclude from the copying.</param>
		public virtual void CopyFrom(TEntity entity, params string[] propertiesToExclude)
		{
			Type dtoType = GetType();
			foreach (PropertyInfo entityPropertyInfo in entity.GetType().GetProperties())
			{
				if (propertiesToExclude.Contains(entityPropertyInfo.Name)) continue;
				if (typeof(ICollection).IsAssignableFrom(entityPropertyInfo.PropertyType)) continue;
				if (entityPropertyInfo.PropertyType.IsGenericType && 
					typeof(ICollection<>).IsAssignableFrom(entityPropertyInfo.PropertyType.GetGenericTypeDefinition())) continue;

				PropertyInfo dtoPropertyInfo = dtoType.GetProperty(entityPropertyInfo.Name);
				if (dtoPropertyInfo == null) continue;

				dtoPropertyInfo.SetValue(this, entityPropertyInfo.GetValue(entity));
			}
		}

		/// <summary>
		/// Copies the value of all entity properties from the data transfer object to the entity.
		/// </summary>
		/// <param name="entity">The entity to copy the property values to.</param>
		/// <param name="propertiesToExclude">The names of any properties to exclude from the copying.</param>
		public virtual void CopyTo(TEntity entity, params string[] propertiesToExclude)
		{
			Type dtoType = GetType();
			foreach (PropertyInfo entityPropertyInfo in entity.GetType().GetProperties())
			{
				if (propertiesToExclude.Contains(entityPropertyInfo.Name)) continue;
				if (typeof(ICollection).IsAssignableFrom(entityPropertyInfo.PropertyType)) continue;
				if (entityPropertyInfo.PropertyType.IsGenericType && 
					typeof(ICollection<>).IsAssignableFrom(entityPropertyInfo.PropertyType.GetGenericTypeDefinition())) continue;

				PropertyInfo dtoPropertyInfo = dtoType.GetProperty(entityPropertyInfo.Name);
				if (dtoPropertyInfo == null) continue;

				entityPropertyInfo.SetValue(entity, dtoPropertyInfo.GetValue(this));
			}
		}
	}
}
