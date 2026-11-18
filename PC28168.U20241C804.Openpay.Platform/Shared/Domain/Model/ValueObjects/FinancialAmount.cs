namespace PC28168.U20241C804.Openpay.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a non-negative monetary amount expressed in a given currency.
/// </summary>
/// <remarks>
///     The amount must be greater than or equal to zero and the currency code must not be null, empty or whitespace.
///     Operations on this value object return new instances and always keep the same currency code.
///     Author: Geraldine Suarez Chinga
/// </remarks>
public sealed record FinancialAmount
{
    /// <summary>
    ///     Initializes a new <see cref="FinancialAmount"/> with the given amount and currency code.
    /// </summary>
    /// <param name="amount">The monetary amount. Must be greater than or equal to zero.</param>
    /// <param name="currencyCode">The currency code of the amount (for example, PEN or USD).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is less than zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="currencyCode"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="currencyCode"/> is empty or whitespace.</exception>
    public FinancialAmount(decimal amount, string currencyCode)
    {
        if (amount < decimal.Zero)
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                "Amount must be greater than or equal to zero.");

        if (currencyCode is null)
            throw new ArgumentNullException(nameof(currencyCode), "CurrencyCode cannot be null.");

        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException("CurrencyCode cannot be empty or whitespace.", nameof(currencyCode));

        Amount = amount;
        CurrencyCode = currencyCode;
    }

    /// <summary>
    ///     Gets the monetary amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    ///     Gets the currency code of the amount.
    /// </summary>
    public string CurrencyCode { get; }

    /// <summary>
    ///     Applies a multiplier to this amount.
    /// </summary>
    /// <param name="factor">The factor to multiply the amount by.</param>
    /// <returns>A new <see cref="FinancialAmount"/> with the multiplied amount and the same currency code.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the resulting amount is less than zero.</exception>
    public FinancialAmount MultiplyBy(decimal factor) => new(Amount * factor, CurrencyCode);
}
