using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class GradeHelper
    {
        public static void Manage(GradesRepository _grades ,long id, string method, List<long> competences)
        {
            switch (method)
            {
                case "AddCompetenceItem":
                    var item=_grades.GetById(id);
                    _grades.UpdateTable(item, competences);
                    break;
                
                case "RemoveGrade":
                    _grades.DeleteById(id);
                    break;
            }
        }
    }
}