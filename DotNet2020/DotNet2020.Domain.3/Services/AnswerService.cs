using DotNet2020.Domain._3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Services
{
    public class AnswerService
    {
        private readonly DbContext _context;
        public AnswerService(DbContext context)
        {
            _context = context;
        }

        public void AddAnswers(List<AnswerModel> answers, AttestationModel model)
        {
            foreach (var answer in answers)
            {
                var attestationAnswerModel = new AttestationAnswerModel();

                attestationAnswerModel.Answer = answer;
                attestationAnswerModel.Attestation = model;

                model.AttestationAnswer.Add(attestationAnswerModel);
                answer.AttestationAnswer.Add(attestationAnswerModel);
                _context.Add(answer);
            }
        }
    }
}
