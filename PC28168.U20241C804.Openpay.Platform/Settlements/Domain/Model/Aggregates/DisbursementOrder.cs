using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U20241C804.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.Aggregates;

/// <summary>
///     Represents an Openpay disbursement order used to settle and transfer funds to an affiliate store.
/// </summary>
/// <remarks>
///     This aggregate root represents a disbursement order.
///     The OrderTrackingCode must be unique within the system.
///     The User is required and cannot exceed 20 characters. The Remarks are optional and cannot exceed 85 characters.
///     When no status is provided, the order is created as Queued.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public partial class DisbursementOrder
{
    /// <summary>
    ///     Maximum length allowed for the operator user identifier.
    /// </summary>
    public const int UserMaxLength = 20;

    /// <summary>
    ///     Maximum length allowed for the remarks.
    /// </summary>
    public const int RemarksMaxLength = 85;

    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected DisbursementOrder()
    {
        OrderTrackingCode = null!;
        AffiliateStoreId = null!;
        ExecutionSchedule = null!;
        TransferCap = null!;
        User = string.Empty;
    }

    /// <summary>
    ///     Creates a new DisbursementOrder from a creation command.
    /// </summary>
    /// <remarks>
    ///     This constructor acts as the command handler for CreateDisbursementOrderCommand.
    ///     It validates the status, user and remarks business rules before initializing the aggregate.
    /// </remarks>
    /// <param name="command">The CreateDisbursementOrderCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command or one of its value objects is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the status is not a defined disbursement status.</exception>
    /// <exception cref="ArgumentException">Thrown when the user or the remarks break their business rules.</exception>
    public DisbursementOrder(CreateDisbursementOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.OrderTrackingCode);
        ArgumentNullException.ThrowIfNull(command.AffiliateStoreId);
        ArgumentNullException.ThrowIfNull(command.ExecutionSchedule);
        ArgumentNullException.ThrowIfNull(command.TransferCap);

        OrderTrackingCode = command.OrderTrackingCode;
        AffiliateStoreId = command.AffiliateStoreId;
        ExecutionSchedule = command.ExecutionSchedule;
        TransferCap = command.TransferCap;
        Status = ValidateStatus(command.Status ?? DisbursementStatus.Queued);
        User = ValidateUser(command.User);
        Remarks = ValidateRemarks(command.Remarks);
    }

    /// <summary>
    ///     Gets the server-generated identifier for this disbursement order.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the Openpay reference that uniquely identifies this disbursement order.
    /// </summary>
    public OpenpayReference OrderTrackingCode { get; private set; }

    /// <summary>
    ///     Gets the identifier of the affiliate store that receives the funds.
    /// </summary>
    public StoreIdentifier AffiliateStoreId { get; private set; }

    /// <summary>
    ///     Gets the date interval in which the disbursement is executed.
    /// </summary>
    public DateInterval ExecutionSchedule { get; private set; }

    /// <summary>
    ///     Gets the maximum amount that can be transferred.
    /// </summary>
    public FinancialAmount TransferCap { get; private set; }

    /// <summary>
    ///     Gets the current status of this disbursement order.
    /// </summary>
    public DisbursementStatus Status { get; private set; }

    /// <summary>
    ///     Gets the identifier of the operator user that registered this disbursement order.
    /// </summary>
    public string User { get; private set; }

    /// <summary>
    ///     Gets the optional remarks about this disbursement order.
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    ///     Validates that the status is a defined disbursement status.
    /// </summary>
    /// <param name="status">The status to validate.</param>
    /// <returns>The validated status.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the status is not defined.</exception>
    private static DisbursementStatus ValidateStatus(DisbursementStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status), status,
                "Status must be Queued, Processed, Cancelled or Failed.");

        return status;
    }

    /// <summary>
    ///     Validates that the user is not null, empty or whitespace and does not exceed the maximum length.
    /// </summary>
    /// <param name="user">The operator user identifier to validate.</param>
    /// <returns>The validated user.</returns>
    /// <exception cref="ArgumentException">Thrown when the user is missing or too long.</exception>
    private static string ValidateUser(string? user)
    {
        if (string.IsNullOrWhiteSpace(user))
            throw new ArgumentException("User is required and cannot be null, empty, or whitespace.", nameof(user));

        if (user.Length > UserMaxLength)
            throw new ArgumentException($"User cannot be longer than {UserMaxLength} characters.", nameof(user));

        return user;
    }

    /// <summary>
    ///     Validates that the optional remarks do not exceed the maximum length.
    /// </summary>
    /// <param name="remarks">The remarks to validate.</param>
    /// <returns>The validated remarks, or null when they were not provided.</returns>
    /// <exception cref="ArgumentException">Thrown when the remarks are too long.</exception>
    private static string? ValidateRemarks(string? remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks)) return null;

        if (remarks.Length > RemarksMaxLength)
            throw new ArgumentException($"Remarks cannot be longer than {RemarksMaxLength} characters.",
                nameof(remarks));

        return remarks;
    }
}
