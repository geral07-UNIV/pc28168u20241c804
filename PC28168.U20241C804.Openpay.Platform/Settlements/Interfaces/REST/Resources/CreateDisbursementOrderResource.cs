using System.ComponentModel.DataAnnotations;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Interfaces.REST.Resources;

/// <summary>
///     Represents the data required to create a disbursement order.
/// </summary>
/// <remarks>
///     The value objects of the aggregate are received as separate primitive attributes.
///     Author: Geraldine Suarez Chinga
/// </remarks>
/// <param name="OrderTrackingCode">The Openpay reference (GUID) that uniquely identifies the order (required)</param>
/// <param name="AffiliateStoreId">The GUID of the affiliate store (required)</param>
/// <param name="ScheduledFrom">The first date of the execution schedule (required)</param>
/// <param name="ScheduledTo">The last date of the execution schedule, later than ScheduledFrom (required)</param>
/// <param name="TransferCapAmount">The maximum amount that can be transferred, zero or greater (required)</param>
/// <param name="TransferCapCurrencyCode">The currency code of the transfer cap (required)</param>
/// <param name="Status">The status of the order: Queued, Processed, Cancelled or Failed (optional, Queued by default)</param>
/// <param name="User">The identifier of the operator user, up to 20 characters (required)</param>
/// <param name="Remarks">Remarks about the order, up to 85 characters (optional)</param>
public record CreateDisbursementOrderResource(
    [Required(ErrorMessage = "RequiredOrderTrackingCode")]
    Guid? OrderTrackingCode,
    [Required(ErrorMessage = "RequiredAffiliateStoreId")]
    Guid? AffiliateStoreId,
    [Required(ErrorMessage = "RequiredScheduledFrom")]
    DateOnly? ScheduledFrom,
    [Required(ErrorMessage = "RequiredScheduledTo")]
    DateOnly? ScheduledTo,
    [Required(ErrorMessage = "RequiredTransferCapAmount")]
    decimal? TransferCapAmount,
    [Required(ErrorMessage = "RequiredTransferCapCurrencyCode")]
    string TransferCapCurrencyCode,
    string? Status,
    [Required(ErrorMessage = "RequiredDisbursementOrderUser")]
    [StringLength(20, ErrorMessage = "InvalidDisbursementOrderUser")]
    string User,
    [StringLength(85, ErrorMessage = "InvalidDisbursementOrderRemarks")]
    string? Remarks);
