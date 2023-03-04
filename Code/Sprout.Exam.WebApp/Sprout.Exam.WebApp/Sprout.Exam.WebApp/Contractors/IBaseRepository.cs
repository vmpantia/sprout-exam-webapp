using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Contractors
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T t);
        Task Delete(object id);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(object id);
        Task Update(object id, object model);
    }
}