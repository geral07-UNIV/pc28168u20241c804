using System.Net.Mime;
using PC28168.U20241C804.Openpay.Platform.Resources;
using PC28168.U20241C804.Openpay.Platform.Settlements.Application.Services;
using PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST;

/// <summary>
///     Disbursement Orders' controller.
/// </summary>
/// <remarks>
///     Exposes the /api/v1/disbursement-orders endpoint to register Openpay disbursement orders.
///     Author: Geraldine Suarez Chinga
/// </remarks>
/// <param name="disbursementOrderCommandService">The DisbursementOrder Command Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Disbursement Orders")]
public class DisbursementOrdersController(
    IDisbursementOrderCommandService disbursementOrderCommandService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<DisbursementOrdersController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a disbursement order.
    /// </summary>
    /// <param name="resource">CreateDisbursementOrderResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created disbursement order, a bad request if a business
    ///     rule is broken, or a conflict if the order tracking code already exists.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(DisbursementOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateDisbursementOrder([FromBody] CreateDisbursementOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createDisbursementOrderCommand =
                CreateDisbursementOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await disbursementOrderCommandService.Handle(createDisbursementOrderCommand,
                cancellationToken);
            return ActionResultFromCreateDisbursementOrderResultAssembler
                .ToActionResultFromCreateDisbursementOrderResult(result, this, localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                resource.OrderTrackingCode);
            return BadRequest(localizer[ToValidationMessageKey(ex)].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                resource.OrderTrackingCode);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Resolves the localized message key that explains a broken business rule.
    /// </summary>
    /// <param name="exception">The argument exception raised by the domain.</param>
    /// <returns>The resource key of the localized validation message.</returns>
    private static string ToValidationMessageKey(ArgumentException exception) =>
        exception.ParamName switch
        {
            "scheduledTo" => "InvalidExecutionSchedule",
            "amount" => "InvalidTransferCapAmount",
            "currencyCode" => "InvalidTransferCapCurrencyCode",
            "status" => "InvalidDisbursementOrderStatus",
            "user" => "InvalidDisbursementOrderUser",
            "remarks" => "InvalidDisbursementOrderRemarks",
            _ => "InvalidDisbursementOrderRequest"
        };
}
