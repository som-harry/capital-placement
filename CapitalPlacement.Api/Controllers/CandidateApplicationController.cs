


namespace CapitalPlacement.Api;

public class CandidateApplicationController : BaseController
{
    private readonly ICandidateApplicationService _candidateApplicationService;
    public CandidateApplicationController(ICandidateApplicationService candidateApplicationService)
    {
        _candidateApplicationService = candidateApplicationService;
    }
    #region Command
    [HttpPost("create-candidate-application")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitCandidateApplication(CandidateApplicationDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _candidateApplicationService.CreateCandidateApplication(requestDto);

        if (response.ResponseCode == ResponseCodes.CREATED)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.DUPLICATE_RESOURCE)
            return BadRequest(response);
        else
            return StatusCode(500, response);

    }

    [HttpPost("update-candidate-application")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdatedCandidateApplicationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatecandidateApplication(UpdatedCandidateApplicationDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var response = await _candidateApplicationService.UpdateCandidateApplication(requestDto);

        if (response.ResponseCode == ResponseCodes.UPDATED)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    [HttpPost("delete-candidate-application")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCandidateApplication(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest($"Error id shouldn't be empty {id}");
        var response = await _candidateApplicationService.DeleteCandidateApplication(id);

        if (response.ResponseCode == ResponseCodes.DELETED)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    #endregion
    #region Query
    [HttpPost("get-all-candidate-application")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UpdatedCandidateApplicationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCandidateApplication(int pageNumber = 1, int pageSize = 50)
    {
        var response = await _candidateApplicationService.GetAllCandidateApplication(pageNumber, pageSize);

        if (response.ResponseCode == ResponseCodes.SUCCESS)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    [HttpPost("get-candidate-application-by-id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UpdatedCandidateApplicationDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCandidateApplicationById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest($"Error id shouldn't be empty {id}");

        var response = await _candidateApplicationService.GetCandidateApplicationById(id);

        if (response.ResponseCode == ResponseCodes.SUCCESS)
            return Ok(response);
        else if (response.ResponseCode == ResponseCodes.NOT_FOUND)
            return NotFound(response);
        else
            return StatusCode(500, response);

    }
    #endregion
}
