namespace DotNet2020.Domain._3.Models
{
    public class AttestationAnswerModel
    {
        public long AttestationId { get; set; }
        public AttestationModel Attestation { get; set; }
        public long AnswerId { get; set; }
        public AnswerModel Answer { get; set; }
    }
}