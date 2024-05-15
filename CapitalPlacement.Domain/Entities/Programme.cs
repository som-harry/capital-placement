
namespace CapitalPlacement.Domain.Entities
{

    public class FieldDetails<T>
    {
        public bool IsHidden { get; set; }
        public bool IsInternal { get; set; }
        public bool IsMandatory { get; set; }
        public T Value { get; set; }
    }

    public class PersonalInformation
    {
        public FieldDetails<string> FirstName { get; set; }
        public FieldDetails<string> LastName { get; set; }
        public FieldDetails<string>  Email { get; set; }
        public FieldDetails<string> Phone { get; set; }
        public FieldDetails<string> Nationality { get; set; }
        public FieldDetails<string> CurrentResidence { get; set; }
        public FieldDetails<string> IdNumber { get; set; }
        public FieldDetails<DateTime> DateOfBirth { get; set; }
        public FieldDetails<string> Gender { get; set; }
       
    }

    public class ProgrammeQuestion
    {
        public string Question { get; set; }
        public string Type { get; set; }
        public string IsDeleted { get; set; }
        public string MaxChoiceAllowed { get; set; }
        public List<string> Choices { get; set; }
        public string IsOtherEnabled { get; set; }
    }

    public class Programme
    {
        [Key]
        public Guid Id { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public List<ProgrammeQuestion> Questions { get; set; }
        public PersonalInformation PersonalInformation { get; set; }
        public bool IsDeleted { get; set; }
    }
}
