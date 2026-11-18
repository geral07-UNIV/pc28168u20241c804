using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
/// <remarks>Author: Geraldine Suarez Chinga</remarks>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Settlements Bounded Context
        builder.Entity<DisbursementOrder>().HasKey(d => d.Id);
        builder.Entity<DisbursementOrder>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<DisbursementOrder>()
            .Property(d => d.OrderTrackingCode)
            .HasConversion(valueObject => valueObject.Value, value => new OpenpayReference(value))
            .IsRequired();
        builder.Entity<DisbursementOrder>()
            .HasIndex(d => d.OrderTrackingCode)
            .IsUnique();
        builder.Entity<DisbursementOrder>()
            .Property(d => d.AffiliateStoreId)
            .HasConversion(valueObject => valueObject.Value, value => new StoreIdentifier(value))
            .IsRequired();
        builder.Entity<DisbursementOrder>().OwnsOne(d => d.ExecutionSchedule, schedule =>
        {
            schedule.WithOwner().HasForeignKey("Id");
            // The MySQL provider reads DATE columns as DateTime, so DateOnly values are converted explicitly.
            schedule.Property(s => s.ScheduledFrom).HasColumnName("ScheduledFrom")
                .HasConversion(date => date.ToDateTime(TimeOnly.MinValue), value => DateOnly.FromDateTime(value))
                .HasColumnType("date")
                .IsRequired();
            schedule.Property(s => s.ScheduledTo).HasColumnName("ScheduledTo")
                .HasConversion(date => date.ToDateTime(TimeOnly.MinValue), value => DateOnly.FromDateTime(value))
                .HasColumnType("date")
                .IsRequired();
        });
        builder.Entity<DisbursementOrder>().Navigation(d => d.ExecutionSchedule).IsRequired();
        builder.Entity<DisbursementOrder>().OwnsOne(d => d.TransferCap, transferCap =>
        {
            transferCap.WithOwner().HasForeignKey("Id");
            transferCap.Property(t => t.Amount).HasColumnName("TransferCapAmount").HasPrecision(18, 2).IsRequired();
            transferCap.Property(t => t.CurrencyCode).HasColumnName("TransferCapCurrencyCode").IsRequired();
        });
        builder.Entity<DisbursementOrder>().Navigation(d => d.TransferCap).IsRequired();
        builder.Entity<DisbursementOrder>().Property(d => d.Status).IsRequired();
        builder.Entity<DisbursementOrder>().Property(d => d.User).IsRequired()
            .HasMaxLength(DisbursementOrder.UserMaxLength);
        builder.Entity<DisbursementOrder>().Property(d => d.Remarks)
            .HasMaxLength(DisbursementOrder.RemarksMaxLength);

        builder.UseSnakeCaseNamingConvention();
    }
}
