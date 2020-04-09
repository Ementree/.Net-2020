using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class CompetenceHelper
    {
        public static void Manage(CompetencesRepository _competences, long id, string method, string contentItem, List<int> checkboxes)
        {
            switch (method)
            {
                case "AddContentItem":
                    var item=_competences.GetById(id);
                    var old = item.Content.ToList();
                    old.Add(contentItem);
                    item.Content = old.ToArray();
                    _competences.Save();
                    break;
                case "RemoveCompetence":
                    _competences.DeleteById(id);
                    break;
                case "RemoveContent":
                    var element=_competences.GetById(id);
                    var oldArr = element.Content;
                    for (int i = 0; i < oldArr.Length; i++)
                    {
                        if (checkboxes.Contains(i))
                            oldArr[i] = null;
                    }
                    oldArr = oldArr.Where(x => x != null).ToArray();
                    element.Content = oldArr;
                    _competences.Save();
                    break;
            }
        }
    }
}