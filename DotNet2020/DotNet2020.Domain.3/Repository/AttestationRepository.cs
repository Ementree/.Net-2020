using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Repository
{
    public class AttestationRepository:IRepository<AttestationModel>
    {
        private readonly AttestationContext _attestation;
        public AttestationRepository(AttestationContext attestation)
        {
            _attestation = attestation;
        }
        public List<AttestationModel> GetList()
        {
            return _attestation.Attestations.ToList();
        }

        public AttestationModel GetById(long id)
        {
            return _attestation.Attestations.Find(id);
        }

        public void Create(AttestationModel item)
        {
            _attestation.Attestations.Add(item);
            Save();
        }

        public void Update(AttestationModel item)
        {
            _attestation.Entry(item).State = EntityState.Modified;
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _attestation.Attestations.Remove(item);
            Save();
        }

        public void Save()
        {
            _attestation.SaveChanges();
        }
    }
}