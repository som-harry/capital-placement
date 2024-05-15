
using CapitalPlacement.Domain;

namespace CapitalPlacement.Application;

public class CandidateApplicationService: ICandidateApplicationService
{
    private readonly ILogger<CandidateApplicationService> _logger;
    private readonly IAsyncRepository<CandidateApplication> _asyncRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CandidateApplicationService(ILogger<CandidateApplicationService> logger,
     IAsyncRepository<CandidateApplication> asyncRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _asyncRepository = asyncRepository;
        _unitOfWork = unitOfWork;
    }

    #region Command
    public async Task<BaseResponse<Guid>> CreateCandidateApplication(CandidateApplicationDto candidateApplicationDto)
    {
        try
        {
            var mappedRequest = candidateApplicationDto.Adapt<CandidateApplication>();
            var existingProgram = await _asyncRepository.SingleOrDefaultAsync(x => !x.IsDeleted &&
                                x.PersonalInformation.Email == candidateApplicationDto.PersonalInformation.Email.Trim());
            if (existingProgram != null)
                return new BaseResponse<Guid>($"Candidate application with Email already exist, please a different Candidate Email", ResponseCodes.DUPLICATE_RESOURCE);

            await _asyncRepository.Add(mappedRequest);
            var itemsCreated = await _unitOfWork.CommitChangesAsync();
            if (itemsCreated <= 0) 
            {
                _logger.LogError("Failed to create response, when trying to save in candidate applicant table");
                return new BaseResponse<Guid>("Failed to create response", ResponseCodes.SERVER_ERROR);
            }

            _logger.LogInformation("successfully added candidate application to the table");
            return new BaseResponse<Guid>($"successfully created ", mappedRequest.Id, ResponseCodes.CREATED);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<Guid>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }
    }

    public async Task<BaseResponse<UpdatedCandidateApplicationDto>> UpdateCandidateApplication(UpdatedCandidateApplicationDto updatedCandidateApplicationDto)
    {
        try
        {
            var mappedRequest = updatedCandidateApplicationDto.Adapt<CandidateApplication>();
            var isDetailExist = await CheckIfDetailExist(mappedRequest);
            if (!isDetailExist.Item1)
            {
                _logger.LogError($"{isDetailExist.Item2}");
                return new BaseResponse<UpdatedCandidateApplicationDto>($"{isDetailExist.Item2}", ResponseCodes.NOT_FOUND);
            }
            _asyncRepository.Update(mappedRequest);
            var itemsCreated = await _unitOfWork.CommitChangesAsync();
            if (itemsCreated <= 0)
            {
                _logger.LogError("Failed to create response, when trying to update in candidate applicant table");
                return new BaseResponse<UpdatedCandidateApplicationDto>("Failed to update response", ResponseCodes.SERVER_ERROR);
            }
            _logger.LogInformation("successfully added applicate to the table");
            var mappedResponse = mappedRequest.Adapt<UpdatedCandidateApplicationDto>();
            return new BaseResponse<UpdatedCandidateApplicationDto>($"successfully updated ", mappedResponse, ResponseCodes.UPDATED);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<UpdatedCandidateApplicationDto>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    public async Task<BaseResponse<string>> DeleteCandidateApplication(Guid id)
    {
        try
        {
            var result = await _asyncRepository.GetById(id);
            if (result is null)
            {
                _logger.LogInformation($"No record was found in the database");
                return new BaseResponse<string>($"No record was found in the database", ResponseCodes.NOT_FOUND);
            }

            result.IsDeleted = true;
            _asyncRepository.Update(result);
            var rowsAffected = await _unitOfWork.CommitChangesAsync();
            if (rowsAffected <= 0)
            {
                _logger.LogError("Failed to  delete record from the table");
                return new BaseResponse<string>($"failed to delete ", ResponseCodes.SERVER_ERROR); 
            }

            _logger.LogInformation("successfully remove application from the table");
            return new BaseResponse<string>($"successfully removed ", $"Application with {id} has been successfully from the database", ResponseCodes.DELETED);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<string>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    #endregion
    #region query
    public async Task<BaseResponse<IEnumerable<UpdatedCandidateApplicationDto>>> GetAllCandidateApplication(int pageNumber, int pageSize)
    {
        try
        {
            var result = await _asyncRepository.GetAll(pageNumber, pageSize,x => !x.IsDeleted);
            if (!result.Any())
            {
                _logger.LogInformation($"No record was found in the database");
                return new BaseResponse<IEnumerable<UpdatedCandidateApplicationDto>>($"No record was found in the database", ResponseCodes.NOT_FOUND);
            }
            _logger.LogInformation("successfully gotten application details");
            var mappedResponse = result.Adapt<IEnumerable<UpdatedCandidateApplicationDto>>();
            return new BaseResponse<IEnumerable<UpdatedCandidateApplicationDto>>($"successfully fetched response ", mappedResponse, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<IEnumerable<UpdatedCandidateApplicationDto>>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    public async Task<BaseResponse<UpdatedCandidateApplicationDto>> GetCandidateApplicationById(Guid id)
    {
        try
        {
            var result = await _asyncRepository.GetById(id);
            if (result is null)
            {
                _logger.LogInformation($"No record was found in the database");
                return new BaseResponse<UpdatedCandidateApplicationDto>($"No record was found in the database", ResponseCodes.NOT_FOUND);
            }
            _logger.LogInformation("successfully gotten response from the table");
            var mappedResponse = result.Adapt<UpdatedCandidateApplicationDto>();
            return new BaseResponse<UpdatedCandidateApplicationDto>($"successfully fetched response ", mappedResponse, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<UpdatedCandidateApplicationDto>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    #endregion
    #region private method
    private async Task<(bool, string)> CheckIfDetailExist(CandidateApplication requeestForm)
    {
        var getApplicateDetail = await _asyncRepository.GetById(requeestForm.Id);
        if (getApplicateDetail == null)
        {
            return (false, "record was not found in the database");
        }
        return (true, "record found in the database");
    }
    #endregion
}
