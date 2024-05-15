using CapitalReplacement.Application.Responses;

namespace CapitalPlacement.Application;

public interface ICandidateApplicationService
{
    Task<BaseResponse<Guid>> CreateCandidateApplication(CandidateApplicationDto candidateApplicationDto);
    Task<BaseResponse<UpdatedCandidateApplicationDto>> UpdateCandidateApplication(UpdatedCandidateApplicationDto updatedCandidateApplicationDto);
    Task<BaseResponse<string>> DeleteCandidateApplication(Guid id);
    Task<BaseResponse<IEnumerable<UpdatedCandidateApplicationDto>>> GetAllCandidateApplication(int pageNumber, int pageSize);
    Task<BaseResponse<UpdatedCandidateApplicationDto>> GetCandidateApplicationById(Guid id);
}
