namespace PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the identifier of an affiliate store.
/// </summary>
/// <remarks>
///     The identifier is a GUID (UUID) that originates in another bounded context.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public sealed record StoreIdentifier
{
    /// <summary>
    ///     Initializes a new <see cref="StoreIdentifier"/> with the given GUID value.
    /// </summary>
    /// <param name="value">The GUID that identifies the affiliate store. Must not be empty.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is an empty GUID.</exception>
    public StoreIdentifier(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("StoreIdentifier cannot be an empty GUID.", nameof(value));

        Value = value;
    }

    /// <summary>
    ///     Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
