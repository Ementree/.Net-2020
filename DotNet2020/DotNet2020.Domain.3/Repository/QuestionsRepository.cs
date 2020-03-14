using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class QuestionsRepository:IRepository<QuestionsModel>
    {
        private readonly QuestionsContext _questions;
        public QuestionsRepository(QuestionsContext questions)
        {
            _questions = questions;
        }
        
        public List<QuestionsModel> GetList()
        {
            return _questions.Questions.ToList();
        }

        public QuestionsModel GetById(long id)
        {
            return _questions.Questions.Find(id);
        }

        public void Create(QuestionsModel item)
        {
            _questions.Questions.Add(item);
            Save();
        }

        public void Update(QuestionsModel item)
        {
            _questions.Entry(item).State = EntityState.Modified;
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if(item!=null)
                _questions.Questions.Remove(item);
            Save();
        }

        public void Save()
        {
            _questions.SaveChanges();
        }
    }
}