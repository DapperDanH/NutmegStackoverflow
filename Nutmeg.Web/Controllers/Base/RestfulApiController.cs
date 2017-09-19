using Nutmeg.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Microsoft.EntityFrameworkCore;

namespace Nutmeg.Controllers
{
	/// <summary>
	/// Adds REST-ful utilities to the base API controller.
	/// </summary>
	/// <typeparam name="TEntity">The type of the main entity associated with the controller.</typeparam>
	/// <typeparam name="TDto">The type of the main data transfer object that corresponds to the main entity.</typeparam>
	/// <typeparam name="TKey">The type of the entity's unique ID property.</typeparam>
	public abstract class RestfulApiController<TEntity, TDto, TKey> : BaseApiController
		where TEntity : class, IKeyed<TKey>, new()
		where TDto : IDtoForEntity<TEntity, TKey>, new()
	{
		#region Fields/Properties
		/// <summary>
		/// Gets the context collection for the main entities associated with the controller.
		/// </summary>
		protected readonly DbSet<TEntity> DbSet;
		#endregion Fields/Properties

		#region Constructors
		protected RestfulApiController(NutmegContext dbContext)
			: base(dbContext)
		{
			DbSet = dbContext.Set<TEntity>();
		}
		#endregion Constructors

		#region Methods
		[HttpGet]
		public virtual async Task<IActionResult> Get()
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Get)}()");

				var entities = await GetEntities();
				_logger.Info(() => $"Returned {entities.Length} entities from the db");

				TDto[] dtos = DtosFromEntities(entities);
				return Ok(dtos);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Get)}()");
				throw;
			}
		}
		protected virtual async Task<TEntity[]> GetEntities()
		{
			return await DbSet.ToArrayAsync();
		}

		[HttpGet("ForLookup")]
		public async Task<IActionResult> ForLookup()
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(ForLookup)}()");

				var entities = await GetEntitiesForLookup();
				_logger.Info(() => $"Returned {entities.Length} entities from the db");

				LookupDto<TEntity, TKey>[] lookupDtos = DtosFromEntityLookups(entities);
				return Ok(lookupDtos);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(ForLookup)}");
				throw;
			}
		}
		protected virtual async Task<TEntity[]> GetEntitiesForLookup()
		{
			return await DbSet.ToArrayAsync();
		}
		protected virtual LookupDto<TEntity, TKey>[] DtosFromEntityLookups(IEnumerable<TEntity> entities)
		{
			return entities.Select(o => o.AsDto<TEntity, LookupDto<TEntity, TKey>, TKey>()).ToArray();
		}

		[HttpGet("{id}")]
		public virtual async Task<IActionResult> Get(TKey id)
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Get)}({nameof(id)} = {id})");

				var entity = await GetEntityById(id);
				if (entity == null)
				{
					_logger.Warn(() => $"Entity not found with id: {id}");
					return NotFound();
				}

				TDto dto = DtoFromEntity(entity);
				return Ok(dto);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Get)}");
				throw;
			}
		}
		protected virtual async Task<TEntity> GetEntityById(TKey id)
		{
			return await DbSet.FindAsync(id);
		}

		[HttpPost]
		public virtual async Task<IActionResult> Post([FromBody]TDto dto)
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Post)}({nameof(dto)} = {JsonConvert.SerializeObject(dto)})");

				TEntity entity = new TEntity();
				AddEntity(entity, dto);

				var result = CanAddEntity(entity, dto);
				if (!result.canAdd)
				{
					_logger.Info(() => "Entity not allowed to be added");
					return BadRequest(result.message);
				}

				await _dbContext.SaveChangesAsync();
				entity = await EntitySaved(entity);

				return Ok(DtoFromEntityOnPost(entity));
			}
			catch (EntityValidationException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Post)}");
				return BadRequest(ex.Message);
			}
			//catch (DbEntityValidationException ex)
			//{
			//	_logger.Error(ex, $"{GetType().Name}.{nameof(Post)}");
			//	return BadRequest();
			//}
			catch (DbUpdateException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Post)}");
				return BadRequest();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Post)}");
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
		protected virtual void AddEntity(TEntity entity, TDto dto)
		{
			dto.CopyTo(entity);
			AssignNewKey(entity);
			DbSet.Add(entity);
		}

		protected virtual (bool canAdd, string message) CanAddEntity(TEntity entity, TDto dto)
		{
			return (true, string.Empty);
		}

		[HttpPut]
		public virtual async Task<IActionResult> Put([FromBody]TDto dto)
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Put)}({nameof(dto)} = {JsonConvert.SerializeObject(dto)})");

				var entity = await GetEntityById(dto.Id);
				if (entity == null)
				{
					_logger.Warn(() => $"Entity not found with id: {dto.Id}");
					return NotFound();
				}

				var result = CanEditEntity(entity, dto);
				if (!result.canEdit)
				{
					_logger.Info(() => "Entity not allowed to be edited");
					return BadRequest(result.message);
				}

				// update the entity from the dto, then save the changes
				UpdateEntity(entity, dto);
				await _dbContext.SaveChangesAsync();
				entity = await EntitySaved(entity);

				return Ok(DtoFromEntity(entity));
			}
			catch (EntityValidationException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Put)}");
				return BadRequest(ex.Message);
			}
			catch (DbUpdateException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Put)}");
				return BadRequest();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Put)}");
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
		protected virtual (bool canEdit, string message) CanEditEntity(TEntity entity, TDto dto)
		{
			return (true, string.Empty);
		}

		protected virtual void UpdateEntity(TEntity entity, TDto dto)
		{
			dto.CopyTo(entity);
		}

		[HttpDelete("{id}")]
		public virtual async Task<IActionResult> Delete(TKey id)
		{
			try
			{
				_logger.Trace(() => $"{GetType().Name}.{nameof(Delete)}({nameof(id)} = {id})");

				var entity = await GetEntityForDeleteById(id);
				if (entity == null)
				{
					_logger.Warn(() => $"Entity not found with id: {id}");
					return Ok();
				}

				string message;
				if (!CanDeleteEntity(entity, out message))
				{
					_logger.Info(() => "Entity not allowed to be deleted");
					return BadRequest(message);
				}

				DeleteEntity(entity);
				await _dbContext.SaveChangesAsync();
				return Ok();
			}
			catch (EntityValidationException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Delete)}");
				return BadRequest(ex.Message);
			}
			catch (DbUpdateException ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Delete)}");
				return BadRequest();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"{GetType().Name}.{nameof(Delete)}");
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		protected virtual bool CanDeleteEntity(TEntity entity, out string message)
		{
			message = null;
			return true;
		}
		protected virtual void DeleteEntity(TEntity entity)
		{
			DbSet.Remove(entity);
		}
		protected virtual async Task<TEntity> GetEntityForDeleteById(TKey id)
		{
			return await GetEntityById(id);
		}

		/// <summary>
		/// Assigns a unique key to the newly created entity (if not already assigned).
		/// </summary>
		/// <param name="entity">The new entity to assign the unique key to.</param>
		protected virtual void AssignNewKey(TEntity entity)
		{
			if (typeof(TKey) == typeof(Guid))
			{
				if (Guid.Empty.Equals(entity.Id))
				{
					entity.Id = (TKey)(object)Guid.NewGuid();
				}
			}
		}

		/// <summary>
		/// Converts the entities to corresponding data transfer objects.
		/// </summary>
		/// <param name="entities">The entities to convert.</param>
		/// <returns>The corresponding data transfer objects.</returns>
		protected virtual TDto[] DtosFromEntities(IEnumerable<TEntity> entities)
		{
			return entities.Select(o => DtoFromEntity(o)).ToArray();
		}

		/// <summary>
		/// Converts the entity to a corresponding data transfer object.
		/// </summary>
		/// <param name="entity">The entity to convert.</param>
		/// <returns>The corresponding data transfer object.</returns>
		protected virtual TDto DtoFromEntity(TEntity entity)
		{
			return entity.AsDto<TEntity, TDto, TKey>();
		}

		protected virtual TDto DtoFromEntityOnPost(TEntity entity)
		{
			return DtoFromEntity(entity);
		}

		/// <summary>
		/// Occures after the entity has been saved to the DB after Add/Edit.
		/// </summary>
		/// <param name="entity">The entity that was saved.</param>
		/// <returns>The Entity.</returns>
		protected virtual async Task<TEntity> EntitySaved(TEntity entity)
		{
			return await Task.FromResult(entity);
		}
		#endregion Methods
	}
}