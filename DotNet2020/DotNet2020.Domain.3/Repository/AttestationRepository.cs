using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class AttestationRepository:IRepository<AttestationModel>
    {
        private readonly AttestationContext _context;
        public AttestationRepository(AttestationContext context)
        {
            _context = context;
        }
        public List<AttestationModel> GetList()
        {
            return _context.Attestations.ToList();
        }

        public AttestationModel GetById(long id)
        {
            return _context.Attestations.Find(id);
        }

        public void Create(AttestationModel item)
        {
            _context.Attestations.Add(item);
            Save();
        }

        public void Update(AttestationModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _context.Attestations.Remove(item);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddAnswersToAttestation(AttestationModel attestationModel, List<AnswerModel> answers)
        {
            if(attestationModel.AttestationAnswer==null)
                attestationModel.AttestationAnswer=new List<AttestationAnswerModel>();
            foreach (var answer in answers)
            {
                _context.Answers.Add(answer);
            }
            Save();
            foreach (var answer in answers)
            {
                attestationModel.AttestationAnswer.Add(new AttestationAnswerModel { AttestationId = attestationModel.Id, AnswerId = answer.AnswerId});
            }
            Save();
        }
        
        /// <summary>
        /// Получить все вопросы по id аттестации
        /// </summary>
        /// <param name="itemId">Id аттестации</param>
        /// <returns>Лист вопросов</returns>
        public List<AnswerModel> GetAllAnswersById(long itemId)
        {
            var unionList = _context.AttestationAnswer.Where(x => x.AnswerId == itemId).ToList();
            var attestationModels=new List<AnswerModel>();
            foreach (var element in unionList)
            {
                attestationModels.Add(_context.Answers.Find(element.AnswerId));
            }

            return attestationModels;
        }
    }
}