using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DotNet2020.Domain._3.Models
{
    public class SpecificWorkerCompetencesModel
    {
        public int WorkerId { get; set; }
        public SpecificWorkerModel Worker { get; set; }
        public long CompetenceId { get; set; }
        public CompetencesModel Competence { get; set; }
    }

    public class SpecificWorkerCompetencesComparer : IEqualityComparer<SpecificWorkerCompetencesModel>
    {
        public bool Equals([AllowNull] SpecificWorkerCompetencesModel x, [AllowNull] SpecificWorkerCompetencesModel y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.CompetenceId == y.CompetenceId && x.WorkerId == y.WorkerId;
        }

        public int GetHashCode([DisallowNull] SpecificWorkerCompetencesModel obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int comeptenceHash = obj.CompetenceId.GetHashCode();

            int workerCash = obj.WorkerId.GetHashCode();

            return comeptenceHash ^ workerCash;
        }
    }
}