using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRepository<T> where T: class,IEntity
    {
        //Eger Async metotlarınız geriye deger döndürecekse Task<> tipinde deger döndürmelidir

        //Eger geriye deger döndürmeyeceklerse (void) Task deger döndürmelilerdir...Async metotlar teknik olarak void olabilir ama geriye deger döndürmeyecekse bile yani void olmasını istiyorsanız bile Task olarak dönen degeri tanımlamalısınız...


        //Queries

        Task<List<T>> GetAllAsync();
        
        
        Task<T> GetByIdAsync(int id);

        IQueryable<T> Where(Expression<Func<T,bool>> exp);

        //_context.Categories.Where(x => x.CategoryName.Contains("asdadasd"))


        // x :  Argüman 

        //  =>(lambda) operatorunden sonrası sizin niyetinizin yani eyleminizin şartının beyan edildigi asamadır...

       

      

        

        
       

        //Commands
        Task CreateAsync(T entity);
        Task UpdateAsync(T originalEntity,T  newEntity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();

    }
}
