using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class WorkerRepository:IRepository<WorkerModel>
    {
        private readonly WorkerContext _workers;

        public WorkerRepository(WorkerContext workers)
        {
            _workers = workers;
        }
        public List<WorkerModel> GetList()
        {
            return _workers.Workers.ToList();
        }

        public WorkerModel GetById(long id)
        {
            return _workers.Workers.Find(id);
        }

        public void Create(WorkerModel item)
        {
            _workers.Workers.Add(item);
            Save();
        }

        public void Update(WorkerModel item)
        {
            _workers.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _workers.Remove(item);
            Save();
        }

        public void Save()
        {
            _workers.SaveChanges();
        }
    }
}