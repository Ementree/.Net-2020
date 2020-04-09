using System.Collections.Generic;
using DotNet2020.Domain._3.Models;

namespace DotNet2020.Domain._3.Repository.Main
{
    public interface ICompetencesGetter<T> where T:class
    {
        public List<CompetencesModel> GetAllCompetencesById(long itemId); //получить все компетенции, которые содержит itemId

        public bool IsAnyCompetences(long itemId); //проверяет, содержиться ли данный itemId в таблице

        public List<long> GetAllItemsThatContainsCompetence(long competenceId); //возвращает лист, в котором содержатся все itemId у которых есть данная компетенция

        public void AddToAnotherTable(T item, List<long> competences);
    }
}