namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Represents the data provided by the server about a disbursement order.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
/// <param name="Id">The server-generated ID of the disbursement order</param>
/// <param name="OrderTrackingCode">The Openpay reference that uniquely identifies the order</param>
/// <param name="AffiliateStoreId">The identifier of the affiliate store</param>
/// <param name="ScheduledFrom">The first date of the execution schedule (yyyy-MM-dd)</param>
/// <param name="ScheduledTo">The last date of the execution schedule (yyyy-MM-dd)</param>
/// <param name="TransferCapAmount">The maximum amount that can be transferred</param>
/// <param name="TransferCapCurrencyCode">The currency code of the transfer cap</param>
/// <param name="Status">The status of the order</param>
/// <param name="User">The identifier of the operator user that registered the order</param>
/// <param name="Remarks">The remarks about the order</param>
/// <param name="UpdatedAt">The date and time of the last update of the record</param>
public record DisbursementOrderResource(
    int Id,
    string OrderTrackingCode,
    string AffiliateStoreId,
    string ScheduledFrom,
    string ScheduledTo,
    decimal TransferCapAmount,
    string TransferCapCurrencyCode,
    string Status,
    string User,
    string? Remarks,
    DateTimeOffset? UpdatedAt);
