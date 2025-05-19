using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRepository<T> where T: class,IEntity
    {
        //Queries

        List<T> GetAll();

        //Commands
    }
}
