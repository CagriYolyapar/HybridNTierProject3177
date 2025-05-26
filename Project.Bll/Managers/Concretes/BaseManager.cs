using Project.Bll.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.Managers.Concretes
{
    public abstract class BaseManager<T> : IManager<T> where T : class, IEntity
    {
        readonly IRepository<T> _repository;

        
        protected BaseManager(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.Status = Entities.Enums.DataStatus.Inserted;
            await _repository.CreateAsync(entity);
        }

        public async Task CreateRangeAsync(List<T> entities)
        {
            foreach (T entity in entities) await CreateAsync(entity);
        }

        public async Task<List<T>> FirstDatas(int count)
        {


            List<T> values = await _repository.GetAllAsync();
            return values.OrderBy(x => x.CreatedDate).Take(count).ToList();
        }

        public List<T> GetActives()
        {
            return _repository.Where(x => x.Status != Entities.Enums.DataStatus.Deleted).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            //Business Logic
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public List<T> GetPassives()
        {
            return _repository.Where(x => x.Status == Entities.Enums.DataStatus.Deleted).ToList();
        }

        public List<T> GetUpdateds()
        {
            return _repository.Where(x => x.Status == Entities.Enums.DataStatus.Updated).ToList();
        }

        public async Task<string> HardDelete(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity.Status == Entities.Enums.DataStatus.Deleted)
            {
                await _repository.DeleteAsync(entity);
                return $"Silme basarılıdır...Silinen id {entity.Id}";
            }
            return "Sadece pasif verileri silebilirsiniz";
        }

        public async Task<List<T>> LastDatas(int count)
        {
            List<T> values = await _repository.GetAllAsync();
            return values.OrderByDescending(x => x.CreatedDate).Take(count).ToList();
        }

        public async Task SoftDelete(int id)
        {
            T entity = await GetByIdAsync(id);

            entity.DeletedDate = DateTime.Now;
            entity.Status = Entities.Enums.DataStatus.Deleted;
            await _repository.SaveChangesAsync();

        }

        public async Task UpdateAsync(T newEntity)
        {
            T originalEntity = await GetByIdAsync(newEntity.Id);
            originalEntity.Status = Entities.Enums.DataStatus.Updated;
            originalEntity.UpdatedDate = DateTime.Now;
            await _repository.UpdateAsync(originalEntity, newEntity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _repository.Where(exp);
        }
    }
}
