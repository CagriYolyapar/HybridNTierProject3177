using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.Managers.Abstracts
{
    public interface IManager<T> where T: class,IEntity
    {
        //BL for Queries
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        List<T> GetActives();
        List<T> GetPassives();
        List<T> GetUpdateds();
        IQueryable<T> Where(Expression<Func<T, bool>> exp);
        Task<List<T>> LastDatas(int count);
        Task<List<T>> FirstDatas(int count);


        //BL for Commands
        Task CreateAsync(T entity);
        Task CreateRangeAsync(List<T> entities);
        Task UpdateAsync(T newEntity);
        Task SoftDelete(int id);
        Task<string> HardDelete(int id);
    }
}
