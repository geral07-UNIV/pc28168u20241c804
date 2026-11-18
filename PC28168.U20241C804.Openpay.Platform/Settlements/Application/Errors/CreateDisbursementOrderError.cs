namespace PC28168.U20241C804.Openpay.Platform.Settlements.Application.Errors;

/// <summary>
///     Represents errors that can occur when creating a disbursement order.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public enum CreateDisbursementOrderError
{
    /// <summary>
    ///     A disbursement order with the same order tracking code already exists.
    /// </summary>
    DuplicateDisbursementOrder,

    /// <summary>
    ///     An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}
