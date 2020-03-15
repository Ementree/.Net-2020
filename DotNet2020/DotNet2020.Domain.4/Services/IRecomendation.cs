using System;
namespace DotNet2020.Domain.Services
{
    public interface IRecomendation
    {
        public string GetRecomendation(int userId);
        public string GetTeamRecomendation(int teamId);
    }
}
