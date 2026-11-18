using PC28168.U20241C804.Openpay.Platform.Resources;
using PC28168.U20241C804.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a disbursement order in the
///     application layer.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public static class ActionResultFromCreateDisbursementOrderResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create disbursement order operation.
    /// </summary>
    /// <param name="result">
    ///     The application result produced by the application layer after handling the command to create a
    ///     disbursement order.
    /// </param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateDisbursementOrderResult(
        Result<DisbursementOrder, CreateDisbursementOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<DisbursementOrder, CreateDisbursementOrderError>.Success success =>
                controller.StatusCode(StatusCodes.Status201Created,
                    DisbursementOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<DisbursementOrder, CreateDisbursementOrderError>.Failure failure =>
                failure.Error switch
                {
                    CreateDisbursementOrderError.DuplicateDisbursementOrder =>
                        controller.Conflict(localizer["DisbursementOrderDuplicated"].Value),

                    CreateDisbursementOrderError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                            statusCode: StatusCodes.Status500InternalServerError),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: StatusCodes.Status500InternalServerError)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError)
        };
}
