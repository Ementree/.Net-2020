using DotNet2020.Domain._3.Models;
using DotNet2020.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._3.Services
{
    public class WorkerService
    {
        private readonly DbContext _context;
        public WorkerService(DbContext context)
        {
            _context = context;
        }

        public SpecificWorkerModel GetWorker(int id)
        {
            var worker = _context
                .Set<SpecificWorkerModel>()
                .Find(id);

            var positionId = _context
                .Entry(worker)
                .Member("PositionId").CurrentValue;

            var position = _context
                .Set<Position>()
                .Find(positionId);

            worker.Position = position;

            _context
                .Entry(worker)
                .Collection(x => x.SpecificWorkerCompetencesModels)
                .Load();

            var competences = _context
                .Set<CompetencesModel>();

            foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
            {
                specificWorkerCompetence.Competence = competences
                    .Find(specificWorkerCompetence.CompetenceId);
            }

            return worker;
        }

        public List<SpecificWorkerModel> GetLoadedWorkers()
        {
            var workers = _context
                .Set<SpecificWorkerModel>()
                .ToList();

            foreach (var worker in workers)
            {
                _context
                    .Entry(worker)
                    .Collection(x => x.SpecificWorkerCompetencesModels)
                    .Load();

                foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
                {
                    specificWorkerCompetence.Competence = _context
                        .Set<CompetencesModel>()
                        .Find(specificWorkerCompetence.CompetenceId);
                }

                var positionId = _context
                    .Entry(worker)
                    .Member("PositionId").CurrentValue;

                var position = _context
                    .Set<Position>()
                    .Find((int)positionId);

                worker.Position = position;
            }

            return workers;
        }

        public Employee TryGetEmployeeWithUsername(string email)
        {
            var employee = _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
            return employee;
        }

        public void GetAddedToWorkerCompetence()
        {

        }
    }
}
