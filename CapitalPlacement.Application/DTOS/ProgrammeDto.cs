
namespace CapitalPlacement.Application.DTOS
{
    public class FieldDetailsDto<T>
    {
        public bool IsHidden { get; set; }
        public bool IsInternal { get; set; }
        public bool IsMandatory { get; set; }
        public T Value { get; set; }
    }

    public class PersonalInformationDto
    {
        [Required]
        public FieldDetailsDto<string> FirstName { get; set; }
        [Required]
        public FieldDetailsDto<string> LastName { get; set; }
        [Required]
        public FieldDetailsDto<string> Email { get; set; }
        public FieldDetailsDto<string> Phone { get; set; }
        public FieldDetailsDto<string> Nationality { get; set; }
        public FieldDetailsDto<string> CurrentResidence { get; set; }
        public FieldDetailsDto<string> IdNumber { get; set; }
        public FieldDetailsDto<DateTime> DateOfBirth { get; set; }
        public FieldDetailsDto<string> Gender { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public string Question { get; set; }
        public string Type { get; set; }
        public string IsDeleted { get; set; }
        public string MaxChoiceAllowed { get; set; }
        public List<string> Choices { get; set; }
        public string IsOtherEnabled { get; set; }
    }

    public class ProgrammeDto
    {

        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public PersonalInformationDto PersonalInformation { get; set; }

    }
    
    public class UpdatedProgrammmeDto : ProgrammeDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
