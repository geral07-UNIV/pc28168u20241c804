using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U20241C804.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles a CreateDisbursementOrderCommand from a CreateDisbursementOrderResource.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public static class CreateDisbursementOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateDisbursementOrderCommand from a CreateDisbursementOrderResource.
    /// </summary>
    /// <remarks>
    ///     The primitive attributes of the resource are turned into the value objects of the domain,
    ///     which validate their own business rules.
    /// </remarks>
    /// <param name="resource">The CreateDisbursementOrderResource resource</param>
    /// <returns>
    ///     A CreateDisbursementOrderCommand assembled from the CreateDisbursementOrderResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value breaks a business rule of a value object.</exception>
    public static CreateDisbursementOrderCommand ToCommandFromResource(CreateDisbursementOrderResource resource) =>
        new(
            new OpenpayReference(resource.OrderTrackingCode.GetValueOrDefault()),
            new StoreIdentifier(resource.AffiliateStoreId.GetValueOrDefault()),
            new DateInterval(resource.ScheduledFrom.GetValueOrDefault(), resource.ScheduledTo.GetValueOrDefault()),
            new FinancialAmount(resource.TransferCapAmount.GetValueOrDefault(), resource.TransferCapCurrencyCode),
            ToStatus(resource.Status),
            resource.User,
            resource.Remarks);

    /// <summary>
    ///     Converts the status text of the resource into a disbursement status.
    /// </summary>
    /// <param name="status">The status name (case-insensitive) or null.</param>
    /// <returns>The parsed status, or null when no status was provided.</returns>
    /// <exception cref="ArgumentException">Thrown when the status is not a valid disbursement status.</exception>
    private static DisbursementStatus? ToStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status)) return null;

        if (Enum.TryParse<DisbursementStatus>(status.Trim(), true, out var parsedStatus) &&
            Enum.IsDefined(parsedStatus))
            return parsedStatus;

        throw new ArgumentException($"Status '{status}' is not a valid disbursement status.", nameof(status));
    }
}
