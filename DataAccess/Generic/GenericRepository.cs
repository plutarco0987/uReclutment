using Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                           string includeProperties = "");
        Task<bool> CreateAsync(T entity);
        bool Update(T entity);
        Task<T> GetById(int id);

        Task<bool> AddLog(string ErrorMessage,string Error);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> whereCondition = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string includeProperties = "")
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<bool> CreateAsync(T entity)
        {
            bool created = false;

            try
            {
                var save = await _unitOfWork.Context.Set<T>().AddAsync(entity);

                if (save != null)
                    created = true;
            }
            catch (Exception)
            {
                throw;
            }
            return created;
        }

        public  bool Update(T entity) 
        {
            bool updated = false;

            try
            {
                var update = _unitOfWork.Context.Set<T>().Update(entity);

                if (update != null)
                    updated = true;
                
            }
            catch (Exception)
            {
                throw;
            }
            return updated;
        }
        public async Task<T> GetById(int id)
        {            
            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                T value= await _unitOfWork.Context.Set<T>().FindAsync(id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                return value;
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception)
            {
                throw;
            }
#pragma warning disable CS0162 // Unreachable code detected
            return null;
#pragma warning restore CS0162 // Unreachable code detected
        }        
        public async Task<bool> AddLog(string ErrorMessage,string Error)
        {
            try
            {
                await _unitOfWork.Context.Set<Log>().AddAsync(new Log(0, ErrorMessage, Error != null ? Error : string.Empty, DateTime.Now, true));
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
