namespace PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Represents the possible states of a disbursement order.
/// </summary>
/// <remarks>
///     Numeric values start at 10. <see cref="Queued"/> is the default state of a new disbursement order.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public enum DisbursementStatus
{
    /// <summary>
    ///     The disbursement order is waiting to be processed. Default state.
    /// </summary>
    Queued = 10,

    /// <summary>
    ///     The disbursement order was processed.
    /// </summary>
    Processed = 11,

    /// <summary>
    ///     The disbursement order was cancelled.
    /// </summary>
    Cancelled = 12,

    /// <summary>
    ///     The disbursement order failed.
    /// </summary>
    Failed = 13
}
