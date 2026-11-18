using PC28168.U20241C804.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241C804.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Application.Services;

/// <summary>
///     Command service interface for disbursement order operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling disbursement order creation commands.
///     It ensures duplicate detection and persistence of new disbursement orders.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public interface IDisbursementOrderCommandService
{
    /// <summary>
    ///     Handle the create disbursement order command.
    /// </summary>
    /// <remarks>
    ///     This method handles the create disbursement order command. It checks if a disbursement order with the
    ///     given OrderTrackingCode already exists. If it does not exist, it creates a new disbursement order and adds
    ///     it to the database.
    /// </remarks>
    /// <param name="command">CreateDisbursementOrderCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created DisbursementOrder on success, or the error reason on failure.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the command breaks a business rule of the aggregate.</exception>
    Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default);
}
