using System.Collections.Generic;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class AttestationHelper
    {
        public static void Attestate(string method, long workerId, List<long> competencesId, CompetencesRepository competences, WorkerRepository workers)
        {
            switch (method)
            {
                case ("Attestation"):
                    
                    break;
                case ("Finished"):
                    break;
            }
        }
    }
}