using System.Collections.Generic;

namespace DotNet2020.Domain._3.Repository.Main
{
    public interface IRepository<T> where T:class
    {
        List<T> GetList();
        T GetById(long id);
        void Create(T item);
        void Update(T item);
        void DeleteById(long id);
        void Save();
    }
}