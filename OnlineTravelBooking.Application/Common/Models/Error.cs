namespace OnlineTravelBooking.Application.Common.Models;

public sealed record Error(string Code, string Message)
{
	public static readonly Error None =
		new(string.Empty, string.Empty);

	public static readonly Error NullValue =
		new("Error.NullValue", "Null value was provided");

	// =========================
	// Validation Errors
	// =========================
	public static Error Validation(string code, string message) =>
		new(code, message);

	// =========================
	// Not Found Errors
	// =========================
	public static Error NotFound(string code, string message) =>
		new(code, message);

	// =========================
	// Conflict Errors (Optional but recommended)
	// =========================
	public static Error Conflict(string code, string message) =>
		new(code, message);
}
