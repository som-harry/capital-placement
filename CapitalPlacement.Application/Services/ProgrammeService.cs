


namespace CapitalPlacement.Application.Services;
public  class ProgrammeService : IProgrammeService
{
    private readonly ILogger<ProgrammeService> _logger;
    private readonly IAsyncRepository<Programme> _asyncRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProgrammeService(ILogger<ProgrammeService> logger, IAsyncRepository<Programme> asyncRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _asyncRepository = asyncRepository;
        _unitOfWork = unitOfWork;
    }
    #region Command
    public async Task<BaseResponse<Guid>>  CreateProgramme(ProgrammeDto applicationFormDto)
    {
        try
        {
            var mappedRequest = applicationFormDto.Adapt<Programme>();
            var existingProgram = await _asyncRepository.SingleOrDefaultAsync(x => !x.IsDeleted && x.ProgramTitle == applicationFormDto.ProgramTitle.Trim());
            if(existingProgram != null)
                return new BaseResponse<Guid>($"Program with title already exist, please a different program title", ResponseCodes.DUPLICATE_RESOURCE);

            await _asyncRepository.Add(mappedRequest);
            var itemsCreated = await _unitOfWork.CommitChangesAsync();
            if(itemsCreated <= 0)  //TODO: log failure
                return new BaseResponse<Guid>("Failed to create response",ResponseCodes.SERVER_ERROR);

            _logger.LogInformation("successfully added applicate to the table");
            return new BaseResponse<Guid>($"successfully created ", mappedRequest.Id, ResponseCodes.CREATED);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<Guid>($"Something went wrong ",  ResponseCodes.SERVER_ERROR); ;
        }

    }
    
    public async Task<BaseResponse<UpdatedProgrammmeDto>> UpdateProgramme(UpdatedProgrammmeDto applicationFormDto)
    {
        try
        {
            var mappedRequest = applicationFormDto.Adapt<Programme>();
            var isDetailExist = await CheckIfDetailExist(mappedRequest);
            if (!isDetailExist.Item1)
            {
                _logger.LogError($"{isDetailExist.Item2}");
                return new BaseResponse<UpdatedProgrammmeDto>($"{isDetailExist.Item2}", ResponseCodes.NOT_FOUND);
            }
             _asyncRepository.Update(mappedRequest);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("successfully added applicate to the table");
            var mappedResponse= mappedRequest.Adapt<UpdatedProgrammmeDto>();
            return new BaseResponse<UpdatedProgrammmeDto>($"successfully created ", mappedResponse, ResponseCodes.UPDATED);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<UpdatedProgrammmeDto>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    public async Task<BaseResponse<string>> DeleteProgramme(Guid id)
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
            if(rowsAffected <=0)
                return new BaseResponse<string>($"failed to delete ", ResponseCodes.SERVER_ERROR);

            _logger.LogInformation("successfully remove application from the table");
            return new BaseResponse<string>($"successfully removed ", $"Application with {id} has been successfully from the database", ResponseCodes.DELETED);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<string>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    #endregion
    #region query
    public async Task<BaseResponse<IEnumerable<UpdatedProgrammmeDto>>> GetAllProgrammes(int pageNumber, int  pageSize)
    {
        try
        {
            var result = await _asyncRepository.GetAll(pageNumber,pageSize, x => !x.IsDeleted);
            if (!result.Any())
            {
                _logger.LogInformation($"No record was found in the database");
                return new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>($"No record was found in the database", ResponseCodes.NOT_FOUND);
            }
            _logger.LogInformation("successfully gotten application details");
            var mappedResponse = result.Adapt<IEnumerable<UpdatedProgrammmeDto>>();
            return new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>($"successfully created ", mappedResponse, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    public async Task<BaseResponse<UpdatedProgrammmeDto>> GetProgrammeById(Guid id)
    {
        try
        {
            var result = await _asyncRepository.GetById(id);
            if (result is null)
            {
                _logger.LogInformation($"No record was found in the database");
                return new BaseResponse<UpdatedProgrammmeDto>($"No record was found in the database", ResponseCodes.NOT_FOUND);
            }
            _logger.LogInformation("successfully added applicate to the table");
            var mappedResponse = result.Adapt<UpdatedProgrammmeDto>();
            return new BaseResponse<UpdatedProgrammmeDto>($"successfully created ", mappedResponse, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured with the error message {ex.Message}", ex);
            return new BaseResponse<UpdatedProgrammmeDto>($"Something went wrong ", ResponseCodes.SERVER_ERROR); ;
        }

    }
    #endregion
    #region private method
    private async Task<(bool,string)> CheckForDuplicate(Programme applicationForm)
    {
        var getApplicateDetail = await _asyncRepository.SingleOrDefaultAsync(fn => fn.PersonalInformation.FirstName == applicationForm.PersonalInformation.FirstName  
        && fn.PersonalInformation.LastName == applicationForm.PersonalInformation.LastName && fn.PersonalInformation.Email == applicationForm.PersonalInformation.Email);
        if(getApplicateDetail != null)
        {
            return (false, "record already exist in the database");
        }
        return (true, "record not found");
    }    
    private async Task<(bool,string)> CheckIfDetailExist(Programme applicationForm)
    {
        var getApplicateDetail = await _asyncRepository.GetById(applicationForm.Id);
        if(getApplicateDetail == null)
        {
            return (false, "record was not found in the database");
        }
        return (true, "record found in the database");
    }
    #endregion
}


