using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Repository.Main;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class CompetencesRepository:IRepository<CompetencesModel>
    {
        private readonly CompetencesContext _competences;
        public CompetencesRepository(CompetencesContext competences)
        {
            _competences = competences;
        }
        public List<CompetencesModel> GetList()
        {
            return _competences.Competences.ToList();
        }

        public CompetencesModel GetById(long id)
        {
            return _competences.Competences.Find(id);
        }

        public void Create(CompetencesModel item)
        {
            _competences.Competences.Add(item);
            Save();
        }

        public void Update(CompetencesModel item)
        {
            _competences.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _competences.Competences.Remove(item);
            Save();
        }

        public void Save()
        {
            _competences.SaveChanges();
        }
    }
}