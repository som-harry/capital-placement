
namespace CapitalReplacement.Api.Controllers;

public class ProgrammeController : BaseController
{
    private readonly ILogger<ProgrammeController> _logger;
    private readonly IProgrammeService _applicationDetailService;
    public ProgrammeController(ILogger<ProgrammeController> logger, IProgrammeService applicationDetailService)
    {
        _logger = logger;
        _applicationDetailService = applicationDetailService;
    }
    #region Command
    [HttpPost("create-programme")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitProgramme(ProgrammeDto applicationFormDto)
    {
        if(!ModelState.IsValid) 
            return BadRequest(ModelState);
        var response = await _applicationDetailService.CreateProgramme(applicationFormDto);

        if(response.ResponseCode == ResponseCodes.CREATED)
            return Ok(response);
        else if(response.ResponseCode == ResponseCodes.DUPLICATE_RESOURCE)      
            return BadRequest(response);
        else
        return StatusCode(500,response);
        

    }
    [HttpPost("update-programme")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdatedProgrammmeDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProgramme(UpdatedProgrammmeDto applicationFormDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _applicationDetailService.UpdateProgramme(applicationFormDto);

        if (response.ResponseCode == ResponseCodes.UPDATED)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500,response);

    }
    [HttpPost("delete-programme")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProgramme(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest($"Error id shouldn't be empty {id}");
        var response = await _applicationDetailService.DeleteProgramme(id);

        if (response.ResponseCode == ResponseCodes.DELETED)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    #endregion
    #region Query
    [HttpPost("get-all-programmes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UpdatedProgrammmeDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProgrammes(int pageNumber = 1, int pageSize = 50)
    {
        var response = await _applicationDetailService.GetAllProgrammes(pageNumber, pageSize);

        if (response.ResponseCode == ResponseCodes.SUCCESS)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    [HttpPost("get-programme-by-id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UpdatedProgrammmeDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProgrammeById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest($"Error id shouldn't be empty {id}");

        var response = await _applicationDetailService.GetProgrammeById   (id);

        if (response.ResponseCode == ResponseCodes.SUCCESS)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500,response);

    }
    #endregion

}

