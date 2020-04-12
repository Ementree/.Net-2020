using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class CompetencesRepository:IRepository<CompetencesModel>
    {
        private readonly AttestationContext _context;
        public CompetencesRepository(AttestationContext context)
        {
            _context = context;
        }
        public List<CompetencesModel> GetList()
        {
            return _context.Competences.ToList();
        }

        public CompetencesModel GetById(long id)
        {
            return _context.Competences.Find(id);
        }

        public void Create(CompetencesModel item)
        {
            _context.Competences.Add(item);
            Save();
        }

        public void Update(CompetencesModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _context.Competences.Remove(item);
            }
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}