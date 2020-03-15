using System;
namespace DotNet2020.Domain.Services
{
    public class Recomendation : IRecomendation
    {
        public Recomendation()
        {
        }

        /// <summary>
        /// Пока достает общую рекомендацию
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetRecomendation(int userId = 0)
        {
            // TODO: Пока будет доставать общую рекомендацию
            throw new NotImplementedException();
        }

        /// <summary>
        /// Достает рекомендацию для команды, метод не реализован
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public string GetTeamRecomendation(int teamId)
        {
            throw new NotImplementedException();
        }
    }
}
