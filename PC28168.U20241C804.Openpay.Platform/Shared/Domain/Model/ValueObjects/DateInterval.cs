namespace PC28168.U20241C804.Openpay.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a closed interval between two calendar dates.
/// </summary>
/// <remarks>
///     The interval is valid only when <see cref="ScheduledTo"/> is later than <see cref="ScheduledFrom"/>.
///     It can be reused by any bounded context that needs to schedule an operation within a date range.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public sealed record DateInterval
{
    /// <summary>
    ///     Initializes a new <see cref="DateInterval"/> with the given start and end dates.
    /// </summary>
    /// <param name="scheduledFrom">The first date of the interval.</param>
    /// <param name="scheduledTo">The last date of the interval. Must be later than <paramref name="scheduledFrom"/>.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="scheduledTo"/> is earlier than or equal to <paramref name="scheduledFrom"/>.
    /// </exception>
    public DateInterval(DateOnly scheduledFrom, DateOnly scheduledTo)
    {
        if (scheduledTo <= scheduledFrom)
            throw new ArgumentException(
                $"ScheduledTo ({scheduledTo:yyyy-MM-dd}) must be later than ScheduledFrom ({scheduledFrom:yyyy-MM-dd}).",
                nameof(scheduledTo));

        ScheduledFrom = scheduledFrom;
        ScheduledTo = scheduledTo;
    }

    /// <summary>
    ///     Gets the first date of the interval.
    /// </summary>
    public DateOnly ScheduledFrom { get; }

    /// <summary>
    ///     Gets the last date of the interval.
    /// </summary>
    public DateOnly ScheduledTo { get; }

    /// <summary>
    ///     Determines whether the interval has already elapsed at the given reference date.
    /// </summary>
    /// <param name="referenceDate">The date used as reference.</param>
    /// <returns>True if <paramref name="referenceDate"/> is later than <see cref="ScheduledTo"/>; otherwise, false.</returns>
    public bool HasElapsed(DateOnly referenceDate) => referenceDate > ScheduledTo;
}
