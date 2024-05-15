
namespace CapitalPlacement.Application.DTOS
{
    public class CandidateApplicationDto
    {
        public Guid ProgrammeId { get; set; }
        public List<QuestionAnswerDto> Answers { get; set; }
        public CandidateInformationDto PersonalInformation { get; set; }
    }

    public class QuestionAnswerDto
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }

    public class CandidateInformationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public string CurrentResidence { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
    public class UpdatedCandidateApplicationDto : CandidateApplicationDto 
    {
        public Guid Id { get; set; }
    }
}
