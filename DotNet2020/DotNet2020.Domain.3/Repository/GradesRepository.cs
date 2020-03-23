using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class GradesRepository:IRepository<GradesModel>, ICompetencesGetter<GradesModel>
    {
        private readonly AttestationContext _context;
        public GradesRepository(AttestationContext context)
        {
            _context = context;
        }
        public List<GradesModel> GetList()
        {
            return _context.Grades.ToList();
        }

        public GradesModel GetById(long id)
        {
            return _context.Grades.Find(id);
        }

        public void Create(GradesModel item)
        {
            _context.Grades.Add(item);
            Save();
        }

        public void Update(GradesModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _context.Grades.Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Получить все компетенции по id
        /// </summary>
        /// <param name="itemId">Id грейда</param>
        /// <returns>Лист компетенций</returns>
        public List<CompetencesModel> GetAllCompetencesById(long itemId)
        {
            var unionList = _context.GradeCompetences.Where(x => x.GradeId == itemId).ToList();
            List<CompetencesModel> competencesModels=new List<CompetencesModel>();
            foreach (var element in unionList)
            {
                competencesModels.Add(_context.Competences.Find(element.CompetenceId));
            }

            return competencesModels;
        }

        /// <summary>
        /// Возвращает true, если у грейда с данным Id есть хотя бы одна компетенция
        /// </summary>
        /// <param name="itemId">Id грейда</param>
        /// <returns>true - хотя бы 1, false - 0</returns>
        public bool IsAnyCompetences(long itemId)
        {
            return _context.GradeCompetences.Any(x=>x.GradeId==itemId);
        }

        /// <summary>
        /// Возвращает лист id грейдов, которые содержат Id данной компетенции
        /// </summary>
        /// <param name="competenceId">id компетенции</param>
        /// <returns></returns>
        public List<long> GetAllItemsThatContainsCompetence(long competenceId)
        {
            List<long> workerIds=new List<long>();
            var unionList = _context.GradeCompetences.Where(x => x.CompetenceId == competenceId).ToList();
            foreach (var element in unionList)
            {
                workerIds.Add(element.GradeId);
            }
            return workerIds;
        }

        public void AddToAnotherTable(GradesModel item, List<long> competences)
        {
            throw new System.NotImplementedException();
        }
        
        public void UpdateTable(GradesModel item, List<long> ids)
        {
            if(item.GradesCompetences==null)
                item.GradesCompetences = new List<GradeCompetencesModel>();
            else
                item.GradesCompetences.Clear();

            var allOldItems = _context.GradeCompetences.Where(x => x.GradeId == item.Id)
                .ToList();

            foreach (var element in allOldItems)
            {
                _context.GradeCompetences.Remove(element);
            }
            
            foreach (var id in ids)
            {
                item.GradesCompetences.Add(new GradeCompetencesModel {GradeId = item.Id, CompetenceId = id});
            }
            Save();
        }
    }
}