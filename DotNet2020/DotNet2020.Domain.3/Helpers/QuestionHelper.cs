using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class QuestionHelper
    {
        public static void Manage(CompetencesRepository _questions, long id, string method, string question, List<int> checkboxes)
        {
            switch (method)
            {
                case "AddQuestion":
                    var item=_questions.GetById(id);
                    var old = item.Questions.ToList();
                    old.Add(question);
                    item.Questions = old.ToArray();
                    _questions.Save();
                    break;
                case "RemoveQuestion":
                    var element=_questions.GetById(id);
                    var oldArr = element.Questions;
                    for (int i = 0; i < oldArr.Length; i++)
                    {
                        if (checkboxes.Contains(i))
                            oldArr[i] = null;
                    }
                    oldArr = oldArr.Where(x => x != null).ToArray();
                    element.Questions = oldArr;
                    _questions.Save();
                    break;
            }
        }
    }
}