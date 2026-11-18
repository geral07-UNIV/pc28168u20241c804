using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241C804.Openpay.Platform.Shared.Domain.Repositories;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Repositories;

/// <summary>
///     The DisbursementOrder repository contract.
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public interface IDisbursementOrderRepository : IBaseRepository<DisbursementOrder>
{
    /// <summary>
    ///     Find a disbursement order by its order tracking code
    /// </summary>
    /// <param name="orderTrackingCode">The Openpay reference to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The disbursement order object if found, or null otherwise.
    /// </returns>
    Task<DisbursementOrder?> FindByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default);
}
