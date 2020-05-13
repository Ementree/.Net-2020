using DotNet2020.Domain._3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._3.Services
{
    public class GradeService
    {
        private readonly DbContext _context;
        public GradeService(DbContext context)
        {
            _context = context;
        }

        public List<GradesModel> GetLoadedGrades()
        {
            var grades = _context.Set<GradesModel>().ToList();
            foreach (var grade in grades)
            {
                _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
                foreach (var gradeCompetences in grade.GradesCompetences)
                {
                    gradeCompetences.Competence = _context.Set<CompetencesModel>().Find(gradeCompetences.CompetenceId);
                }
            }
            return grades;
        }

        public GradeUpdateModel GetGradeUpdateModelFor(long id)
        {
            var grade = _context.Set<GradesModel>().Find(id); //получаю грейд по id
            _context.Entry(grade).Collection(x => x.GradesCompetences).Load(); //загружаю компетенции
            var gradeUpdateModel = new GradeUpdateModel { GradeModel = grade, Competences = _context.Set<CompetencesModel>().ToList() }; //создаю модель грейда
            gradeUpdateModel.Grades = GetLoadedGrades(); //получаю все грейды для связей грейдов
            gradeUpdateModel.Grades.Remove(grade); //из всех грейдов для связей удаляю текущий грейд
            gradeUpdateModel.GradeToGrades = _context.Set<GradeToGradeModel>().ToList(); //получаю таблицу gradeToGrade
            return gradeUpdateModel;
        }

        public void UpdateCompetencesForGrade(GradeUpdateModel gradeUpdateModel, long id)
        {
            var grade = _context.Set<GradesModel>().Find(id);
            var oldGradesCompetences = _context.Set<GradeCompetencesModel>().Where(x => x.GradeId == id);
            _context.Set<GradeCompetencesModel>().RemoveRange(oldGradesCompetences);

            foreach (var competenceId in gradeUpdateModel.NewCompetencesIds)
            {
                var gradeCompetence = new GradeCompetencesModel();

                gradeCompetence.Competence = _context.Set<CompetencesModel>().Find(competenceId);
                gradeCompetence.Grade = grade;

                grade.GradesCompetences.Add(gradeCompetence);

                _context.Set<GradeCompetencesModel>().Add(gradeCompetence);
            }
            _context.SaveChanges();
        }

        public void RemoveGrade(long id)
        {
            var grade = _context.Set<GradesModel>().Find(id);
            var needToDelete = _context.Set<GradeToGradeModel>()
                    .Where(x => x.GradeId == grade.Id || x.NextGradeId == grade.Id);
            _context.Set<GradeToGradeModel>().RemoveRange(needToDelete);
            _context.Set<GradesModel>().Remove(grade);
            _context.SaveChanges();
        }

        public void AddNextGrade(GradeUpdateModel gradeUpdateModel, long id)
        {
            var oldGrades = _context.Set<GradeToGradeModel>().Where(x => x.GradeId == id);
            _context.Set<GradeToGradeModel>().RemoveRange(oldGrades);
            foreach (var newGradeId in gradeUpdateModel.NewGradesIds)
            {
                _context.Set<GradeToGradeModel>().Add(new GradeToGradeModel { GradeId = id, NextGradeId = newGradeId });
            }
            _context.SaveChanges();
        }

        public string GetNextGrades(List<GradesModel> nextGrades)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in nextGrades)
            {
                builder.Append(item.Grade + "\n");
            }
            return builder.ToString();
        }

        public List<GradesModel> GetLowerGrades(long gradeId, List<GradesModel> grades, List<GradeToGradeModel> links)
        {
            var neededLinks = links.Where(l => l.NextGradeId == gradeId);
            var result = new List<GradesModel>();
            foreach (var link in neededLinks)
            {
                var currentGrade = grades.FirstOrDefault(g => g.Id == link.GradeId);
                if (!result.Contains(currentGrade))
                {
                    result.Add(currentGrade);
                    result.AddRange(GetLowerGrades(currentGrade.Id, grades, links));
                }
            }

            return result;
        }

        public List<CompetencesModel> GetLowerCompetences(long gradeId, List<GradesModel> grades, List<GradeToGradeModel> links)
        {
            var neededGrades = GetLowerGrades(gradeId, grades, links);
            neededGrades.Add(grades.FirstOrDefault(x => x.Id == gradeId));
            var result = new List<CompetencesModel>();
            foreach (var grade in neededGrades)
            {
                var gradeCompetences = grade.GradesCompetences;
                foreach (var competence in gradeCompetences.Where(competence => !result.Contains(competence.Competence)))
                    result.Add(competence.Competence);
            }

            return result;
        }

        public List<GradesModel> GetWorkerGrades(List<CompetencesModel> workerCompetences, List<GradesModel> grades,
            List<GradeToGradeModel> links)
        {
            var result = new List<GradesModel>();
            foreach (var grade in grades)
            {
                var gradeCompetences = GetLowerCompetences(grade.Id, grades, links);
                if (!gradeCompetences.Except(workerCompetences).Any()) //чекает что у рабочего есть все компетенции грейда
                    result.Add(grade);
            }
            return result;
        }
    }
}
