using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Models
{
    public static class WorkerOutputModelHelper
    {
        public static List<WorkerOutputModel> GetList(SpecificWorkerRepository workersRepository)
        {
            List<WorkerOutputModel> list=new List<WorkerOutputModel>();
            foreach (var worker in workersRepository.GetList())
            {
                var buf = workersRepository.GetAllCompetencesById(worker.Id);
                list.Add(new WorkerOutputModel {Worker = worker, Competences = buf});
            }

            return list;
        }
    }

    public class WorkerOutputModel
    {
        public SpecificWorkerModel Worker { get; set; }
        public List<CompetencesModel> Competences { get; set; }
    }
}