namespace System;

/// <summary>
/// Provides with extension methods on <see cref="ReadOnlySpan{T}"/>.
/// </summary>
/// <seealso cref="ReadOnlySpan{T}"/>
public static class SpanExtensions
{
	/// <summary>
	/// Determine whether all elements in a <see cref="ReadOnlySpan{T}"/> satisfy the specified condition.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The sequence.</param>
	/// <param name="predicate">The predicate to be checked.</param>
	/// <returns>A <see cref="bool"/> result.</returns>
	public static bool All<T>(this ReadOnlySpan<T> @this, Func<T, bool> predicate)
	{
		foreach (ref readonly var element in @this)
		{
			if (!predicate(element))
			{
				return false;
			}
		}
		return true;
	}
}
