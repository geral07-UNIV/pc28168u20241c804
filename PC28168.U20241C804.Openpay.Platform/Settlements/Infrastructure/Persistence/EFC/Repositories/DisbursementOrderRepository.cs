using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Entity Framework repository for disbursement order persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with disbursement order specific query methods.
///     Author: Geraldine Suarez Chinga
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class DisbursementOrderRepository(AppDbContext context)
    : BaseRepository<DisbursementOrder>(context), IDisbursementOrderRepository
{
    /// <inheritdoc />
    public async Task<DisbursementOrder?> FindByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<DisbursementOrder>()
            .FirstOrDefaultAsync(d => d.OrderTrackingCode == orderTrackingCode, cancellationToken);
    }
}
