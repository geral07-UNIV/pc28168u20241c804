using System.Globalization;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Transform;

/// <summary>
///     Assembles a DisbursementOrderResource from a DisbursementOrder.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public static class DisbursementOrderResourceFromEntityAssembler
{
    private const string DateFormat = "yyyy-MM-dd";

    /// <summary>
    ///     Assembles a DisbursementOrderResource from a DisbursementOrder.
    /// </summary>
    /// <param name="entity">The DisbursementOrder entity</param>
    /// <returns>
    ///     A DisbursementOrderResource assembled from the DisbursementOrder
    /// </returns>
    public static DisbursementOrderResource ToResourceFromEntity(DisbursementOrder entity) =>
        new(
            entity.Id,
            entity.OrderTrackingCode.ToString(),
            entity.AffiliateStoreId.ToString(),
            entity.ExecutionSchedule.ScheduledFrom.ToString(DateFormat, CultureInfo.InvariantCulture),
            entity.ExecutionSchedule.ScheduledTo.ToString(DateFormat, CultureInfo.InvariantCulture),
            entity.TransferCap.Amount,
            entity.TransferCap.CurrencyCode,
            entity.Status.ToString(),
            entity.User,
            entity.Remarks,
            entity.UpdatedAt);
}
