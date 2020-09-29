using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.Repository.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);
        Task<IList<T>> FindByUserName(string UserName);
        Task<bool> isExist(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();

    }
}
