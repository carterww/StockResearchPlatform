using System;
using StockResearchPlatform.Data;

namespace StockResearchPlatform.Repositories
{
	/// <summary>
	/// Class that implements generic CRUD Operations.
	/// </summary>
	/// <typeparam name="T">
	///	Class that is registered with the Dbset
	/// </typeparam>
	public abstract class BaseRepository<T> where T : class
	{
		protected readonly ApplicationDbContext _context;

		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Creates a record in database of type T
		/// </summary>
		/// <param name="item">Object of type T to create</param>
		public virtual void Create(T item)
		{
			_context.Set<T>().Add(item);
            this.Save();
        }

		/// <summary>
		/// Retrieves a record from the database of type T.
		/// Subclass must implement this method because Key column is different for every Dbset type.
		/// </summary>
		/// <param name="item">Object of type T to retrive</param>
		/// <returns>The full record from the database.</returns>
		public abstract T Retrieve(T item);

		/// <summary>
		/// Rerieves a List of objects of type T based on a filter function.
		/// </summary>
		/// <param name="filterFunc">Function used as Entity Core Where function to filter the data</param>
		/// <returns>A list of records from the databse</returns>
		public virtual List<T> Retrieve(Func<T, bool> filterFunc)
		{
			return _context.Set<T>()
				.Where(filterFunc).ToList();
        }

		/// <summary>
		/// Updates an item in the database.
		/// </summary>
		/// <param name="item">Item to be updated</param>
		public virtual void Update(T item)
		{
			_context.Set<T>()
				.Update(item);
            this.Save();
        }

		/// <summary>
		/// Deletes a record from the database
		/// </summary>
		/// <param name="item">Item to be deleted</param>
		public virtual void Delete(T item)
		{
			_context.Set<T>()
				.Remove(item);
			this.Save();
		}

		protected virtual void Save()
		{
			_context.SaveChanges();
		}

		protected async virtual Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}

