using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class GradesRepository:IRepository<GradesModels>
    {
        private readonly GradesContext _grades;
        public GradesRepository(GradesContext grades)
        {
            _grades = grades;
        }

        public List<GradesModels> GetList()
        {
            return _grades.Grades.ToList();
        }

        public GradesModels GetById(long id)
        {
            return _grades.Grades.Find(id);
        }

        public void Create(GradesModels item)
        {
            _grades.Grades.Add(item);
            Save();
        }

        public void Update(GradesModels item)
        {
            _grades.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _grades.Grades.Remove(item);
        }

        public void Save()
        {
            _grades.SaveChanges();
        }
    }
}