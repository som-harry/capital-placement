using CapitalPlacement.Domain.Entities;

namespace CapitalPlacement.Domain;

public class CandidateApplication 
{
    public Guid Id { get; set; }
    public Guid ProgrammeId {get;set;}
    public List<QuestionAnswer> Answers {get;set;}
    public CandidateInformation PersonalInformation { get; set; }
    public bool IsDeleted { get; set; }
}

public class QuestionAnswer
{
    public string Question {get;set;}

    public string Answer {get; set;}
}

public class CandidateInformation 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string  Email { get; set; }
    public string Phone { get; set; }
    public string Nationality { get; set; }
    public string CurrentResidence { get; set; }
    public string IdNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
}