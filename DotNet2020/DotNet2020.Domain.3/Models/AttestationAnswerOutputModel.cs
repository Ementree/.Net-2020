using System.Collections.Generic;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Models
{
    public static class AttestationAnswerOutputModelHelper
    {
        public static List<AttestationAnswerOutputModel> GetList(AttestationRepository attestationRepository)
        {
            var list=new List<AttestationAnswerOutputModel>();
            foreach (var attestation in attestationRepository.GetList())
            {
                var buf = attestationRepository.GetAllAnswersById(attestation.Id);
                list.Add(new AttestationAnswerOutputModel() {Attestation = attestation, Answers = buf});
            }
            return list;
        }
    }
    
    public class AttestationAnswerOutputModel
    {
        public AttestationModel Attestation { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }  
}