using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class SpecificWorkerRepository:IRepository<SpecificWorkerModel>, ICompetencesGetter<SpecificWorkerModel>
    {
        private readonly AttestationContext _context;
        public SpecificWorkerRepository(AttestationContext context)
        {
            _context = context;
        }
        public List<SpecificWorkerModel> GetList()
        {
            return _context.Workers.ToList();
        }

        public SpecificWorkerModel GetById(long id)
        {
            return _context.Workers.Find(id);
        }

        public void Create(SpecificWorkerModel item)
        {
            _context.Workers.Add(item);
            Save();
        }

        public void Update(SpecificWorkerModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _context.Workers.Remove(item);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Получить все компетенции по id
        /// </summary>
        /// <param name="itemId">Id работника</param>
        /// <returns>Лист компетенций</returns>
        public List<CompetencesModel> GetAllCompetencesById(long itemId)
        {
            var unionList = _context.SpecificWorkerCompetences.Where(x => x.WorkerId == itemId).ToList();
            List<CompetencesModel> competencesModels=new List<CompetencesModel>();
            foreach (var element in unionList)
            {
                competencesModels.Add(_context.Competences.Find(element.CompetenceId));
            }

            return competencesModels;
        }
        
        /// <summary>
        /// Получить id всех компетенций по id
        /// </summary>
        /// <param name="itemId">Id работника</param>
        /// <returns>Лист компетенций</returns>
        public List<long> GetAllCompetencesIdsById(long itemId)
        {
            var unionList = _context.SpecificWorkerCompetences.Where(x => x.WorkerId == itemId).ToList();
            var idsList=new List<long>();
            foreach (var element in unionList)
            {
                idsList.Add(element.CompetenceId);
            }

            return idsList;
        }

        /// <summary>
        /// Возвращает true, если у работника с данным Id есть хотя бы одна компетенция
        /// </summary>
        /// <param name="itemId">Id работника</param>
        /// <returns>true - хотя бы 1, false - 0</returns>
        public bool IsAnyCompetences(long itemId)
        {
            return _context.SpecificWorkerCompetences.Any(x=>x.WorkerId==itemId);
        }

        /// <summary>
        /// Возвращает лист id работников, которые содержат Id данной компетенции
        /// </summary>
        /// <param name="competenceId">id компетенции</param>
        /// <returns></returns>
        public List<long> GetAllItemsThatContainsCompetence(long competenceId)
        {
            List<long> workerIds=new List<long>();
            var unionList = _context.SpecificWorkerCompetences.Where(x => x.CompetenceId == competenceId).ToList();
            foreach (var element in unionList)
            {
                workerIds.Add(element.WorkerId);
            }
            return workerIds;
        }

        public void AddToAnotherTable(SpecificWorkerModel item, List<long> competences)
        {
            if(item.SpecificWorkerCompetencesModels==null)
                item.SpecificWorkerCompetencesModels=new List<SpecificWorkerCompetencesModel>();
            foreach (var element in competences)
            {
                item.SpecificWorkerCompetencesModels.Add(new SpecificWorkerCompetencesModel
                    {WorkerId = item.Id, CompetenceId = element});
            }
        }

        public void UpdateTable(SpecificWorkerModel item, List<long> ids)
        {
            if(item.SpecificWorkerCompetencesModels==null)
                item.SpecificWorkerCompetencesModels=new List<SpecificWorkerCompetencesModel>();
            else
                item.SpecificWorkerCompetencesModels.Clear();

            var allOldItems = _context.SpecificWorkerCompetences
                .Where(x => x.WorkerId == item.Id)
                .ToList();

            foreach (var element in allOldItems)
            {
                _context.SpecificWorkerCompetences.Remove(element);
            }
            
            foreach (var id in ids)
            {
                item.SpecificWorkerCompetencesModels.Add(new SpecificWorkerCompetencesModel {WorkerId = item.Id, CompetenceId = id});
            }
            Save();
        }

        public void SaveUpdateTable(SpecificWorkerModel item, List<long> ids)
        {
            if(item.SpecificWorkerCompetencesModels==null)
                item.SpecificWorkerCompetencesModels=new List<SpecificWorkerCompetencesModel>();

            var old = _context.SpecificWorkerCompetences.ToList()
                .Where(x=>x.WorkerId==item.Id)
                .ToList();
            
            
            foreach (var element in old)
            {
                if(ids.Contains(element.CompetenceId))
                    ids.Remove(element.CompetenceId);
            }
            
            foreach (var id in ids)
            {
                item.SpecificWorkerCompetencesModels.Add(new SpecificWorkerCompetencesModel {WorkerId = item.Id, CompetenceId = id});
            }
            Save();
        }
    }
}