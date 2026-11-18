using PC28168.U20241C804.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U20241C804.Openpay.Platform.Settlements.Application.Services;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U20241C804.Openpay.Platform.Shared.Application.Patterns;
using PC28168.U20241C804.Openpay.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling disbursement order creation commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for creating new disbursement orders.
///     It enforces duplicate detection through both application-level checks and database constraints,
///     then coordinates with the unit of work to persist changes.
///     Business rule violations raised by the aggregate are propagated as <see cref="ArgumentException"/>
///     so the interface layer can answer with a bad request.
///     Author: Geraldine Suarez Chinga
/// </remarks>
/// <param name="disbursementOrderRepository">Repository for accessing disbursement order data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class DisbursementOrderCommandService(
    IDisbursementOrderRepository disbursementOrderRepository,
    IUnitOfWork unitOfWork,
    ILogger<DisbursementOrderCommandService> logger)
    : IDisbursementOrderCommandService
{
    /// <inheritdoc />
    public async Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(
        CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var existingDisbursementOrder =
            await disbursementOrderRepository.FindByOrderTrackingCodeAsync(command.OrderTrackingCode,
                cancellationToken);
        if (existingDisbursementOrder != null)
        {
            logger.LogWarning("Duplicate disbursement order rejected for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateDisbursementOrder);
        }

        // Business rules are validated by the aggregate; an ArgumentException is handled by the interface layer.
        var disbursementOrder = new DisbursementOrder(command);

        try
        {
            await disbursementOrderRepository.AddAsync(disbursementOrder, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Success(disbursementOrder);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Duplicate key violation creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateDisbursementOrder);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex,
                "Database update failed creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.UnexpectedError);
        }
    }

    /// <summary>
    ///     Determines whether a DbUpdateException represents a duplicate key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a MySQL duplicate key error (code 1062), false otherwise.</returns>
    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "MySqlException", StringComparison.Ordinal)) continue;
            var numberProperty = current.GetType().GetProperty("Number");
            if (numberProperty?.PropertyType == typeof(int) &&
                numberProperty.GetValue(current) is int errorCode &&
                errorCode == 1062)
                return true;
        }

        return false;
    }
}
