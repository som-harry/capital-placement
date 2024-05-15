using CapitalReplacement.Application.Responses;

namespace CapitalPlacement.Application.Interfaces.Application;

public interface IProgrammeService
{
    Task<BaseResponse<Guid>> CreateProgramme(ProgrammeDto applicationFormDto);
    Task<BaseResponse<UpdatedProgrammmeDto>> UpdateProgramme(UpdatedProgrammmeDto applicationFormDto);
    Task<BaseResponse<string>> DeleteProgramme(Guid id);
    Task<BaseResponse<IEnumerable<UpdatedProgrammmeDto>>> GetAllProgrammes(int pageNumber, int pageSize);
    Task<BaseResponse<UpdatedProgrammmeDto>> GetProgrammeById(Guid id);
}

