using System;
using System.Collections.Generic;
using System.Linq;

namespace Nutmeg.Data
{
	public static class EntityExtensions
	{
		/// <summary>
		/// Returns a new data transfer object with the property values of the specified entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <typeparam name="TDto">The type of the data transfer object.</typeparam>
		/// <typeparam name="TKey">The type of the entity's ID property.</typeparam>
		/// <param name="entity">The entity for which to return a new data transfer object.</param>
		/// <returns>A new data transfer object with the property values of the specified entity.</returns>
		public static TDto AsDto<TEntity, TDto, TKey>(this TEntity entity)
			where TDto : IDtoForEntity<TEntity, TKey>, new()
		{
			if (entity == null) return default(TDto);

			var dto = new TDto();
			dto.CopyFrom(entity);
			return dto;
		}
		/// <summary>
		/// Returns a collection of new data transfer objects with the property values of the specified entities.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entities.</typeparam>
		/// <typeparam name="TDto">The type of the data transfer objects.</typeparam>
		/// <typeparam name="TKey">The type of the entity's ID property.</typeparam>
		/// <param name="entities">The entities for which to return new data transfer objects.</param>
		/// <returns>A collection of new data transfer objects with the property values of the specified entities.</returns>
		public static IEnumerable<TDto> AsDtos<TEntity, TDto, TKey>(this IEnumerable<TEntity> entities)
			where TDto : IDtoForEntity<TEntity, TKey>, new()
		{
			if (entities == null) return Enumerable.Empty<TDto>();

			return entities.Select(o => o.AsDto<TEntity, TDto, TKey>());
		}

		/// <summary>
		/// Adds the entity to the specified context, putting it into the Added state such that it will be inserted into the database when SaveChanges is 
		/// called.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity to add.</param>
		/// <param name="context">The context to add the entity to.</param>
		/// <returns>The entity being added.</returns>
		public static TEntity AddTo<TEntity>(this TEntity entity, NutmegContext context)
			where TEntity : class
		{
			context.Set<TEntity>().Add(entity);
			return entity;
		}
	}
}
