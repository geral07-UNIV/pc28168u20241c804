namespace PC28168.U20241C804.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the Openpay reference that uniquely identifies a disbursement order.
/// </summary>
/// <remarks>
///     The reference is a GUID generated in another bounded context and used as the order tracking code.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public sealed record OpenpayReference
{
    /// <summary>
    ///     Initializes a new <see cref="OpenpayReference"/> with the given GUID value.
    /// </summary>
    /// <param name="value">The GUID that identifies the disbursement order. Must not be empty.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is an empty GUID.</exception>
    public OpenpayReference(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OpenpayReference cannot be an empty GUID.", nameof(value));

        Value = value;
    }

    /// <summary>
    ///     Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
