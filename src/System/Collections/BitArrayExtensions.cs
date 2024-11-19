namespace System.Collections;

/// <summary>
/// Provides with extension methods on <see cref="BitArray"/>.
/// </summary>
/// <seealso cref="BitArray"/>
public static class BitArrayExtensions
{
	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool SequenceEqual(this BitArray @this, [NotNullWhen(true)] BitArray? other)
		=> other is not null
		&& @this.Length == other.Length
		&& Entry.GetArrayField(@this).AsSpan().SequenceEqual(Entry.GetArrayField(other));

	/// <summary>
	/// Get the cardinality of the specified <see cref="BitArray"/>.
	/// </summary>
	/// <param name="this">The array.</param>
	/// <returns>The total number of bits set <see langword="true"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetCardinality(this BitArray @this) => Entry.GetArrayField(@this).Sum(int.PopCount);

	/// <summary>
	/// Try to get internal array field.
	/// </summary>
	/// <param name="this">The array.</param>
	/// <returns>The field.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int[] GetInternalArrayField(this BitArray @this) => Entry.GetArrayField(@this);
}

/// <summary>
/// Represents an entry to call internal fields on <see cref="BitArray"/>.
/// </summary>
/// <seealso cref="BitArray"/>
file static class Entry
{
	/// <summary>
	/// Try to fetch the internal field <c>m_array</c> in type <see cref="BitArray"/>.
	/// </summary>
	/// <param name="this">The list.</param>
	/// <returns>The reference to the internal field.</returns>
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "m_array")]
	public static extern ref int[] GetArrayField(BitArray @this);
}
