using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class GradeHelper
    {
        public static void Manage(GradesRepository _grades, long id, string method, string contentItem, List<int> checkboxes)
        {
            switch (method)
            {
                case "AddCompetenceItem":
                    var item=_grades.GetById(id);
                    var old = item.Competences.ToList();
                    old.Add(contentItem);
                    item.Competences = old.ToArray();
                    _grades.Save();
                    break;
                case "RemoveGrade":
                    _grades.DeleteById(id);
                    break;
                case "RemoveCompetence":
                    var element=_grades.GetById(id);
                    var oldArr = element.Competences;
                    for (int i = 0; i < oldArr.Length; i++)
                    {
                        if (checkboxes.Contains(i))
                            oldArr[i] = null;
                    }
                    oldArr = oldArr.Where(x => x != null).ToArray();
                    element.Competences = oldArr;
                    _grades.Save();
                    break;
            }
        }
    }
}