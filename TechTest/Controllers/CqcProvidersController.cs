using Mapster;
using Microsoft.AspNetCore.Mvc;
using TechTest.Application.Clients.Exceptions;
using TechTest.Application.Services;
using TechTest.Controllers.Models;

namespace TechTest.Controllers
{
    [ApiController]
    [Route("api/providers")]
    public class CqcProvidersController(ICqcProviderService cqcProviderService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetProvidersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetProvidersResponse>> GetProviders()
        {
            try
            {
                var providers = await cqcProviderService.GetAllProvidersAsync();
                var providerDtos = providers.Adapt<List<ProviderSummaryDto>>();

                var response = new GetProvidersResponse();
                response.Providers.AddRange(providerDtos);

                return Ok(response);
            }
            catch (CqcClientRequestException ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: ex.StatusCode,
                    title: "CQC client error");
            }
            catch (Exception)
            {
                return Problem(
                    detail: "An unexpected error has occurred.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetProviderByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetProviderByIdResponse>> GetProviderByIdAsync(string id)
        {
            try
            {
                var provider = await cqcProviderService.GetProviderAsync(id);

                return Ok(provider.Adapt<GetProviderByIdResponse>());
            }
            catch (CqcClientRequestException ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: ex.StatusCode,
                    title: "CQC client error");
            }
            catch (Exception)
            {
                return Problem(
                    detail: "An unexpected error has occurred.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected error");
            }
        }
    }
}
