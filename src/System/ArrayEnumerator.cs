namespace System;

/// <summary>
/// Represents an enumerator type that iterates on each element of an array of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of each element.</typeparam>
public ref struct ArrayEnumerator<T> : IEnumerator<T>
{
	/// <summary>
	/// Indicates the target array.
	/// </summary>
	private readonly T[] _array;

	/// <summary>
	/// Indicates the index.
	/// </summary>
	private int _index;


	/// <summary>
	/// Initializes an <see cref="ArrayEnumerator{T}"/> instance via a list of elements.
	/// </summary>
	/// <param name="array">An array.</param>
	public ArrayEnumerator(T[] array) => (_array, _index) = (array, -1);


	/// <inheritdoc cref="IEnumerator{T}.Current"/>
	public readonly ref readonly T Current => ref _array[_index];

	/// <inheritdoc/>
	readonly object? IEnumerator.Current => Current;

	/// <inheritdoc/>
	readonly T IEnumerator<T>.Current => Current;


	/// <inheritdoc/>
	[DoesNotReturn]
	public override readonly bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <inheritdoc/>
	public bool MoveNext() => ++_index < _array.Length;

	/// <inheritdoc/>
	[DoesNotReturn]
	public override readonly int GetHashCode() => throw new NotSupportedException();

	/// <inheritdoc/>
	[DoesNotReturn]
	public override readonly string ToString() => throw new NotSupportedException();

	/// <inheritdoc/>
	readonly void IDisposable.Dispose() { }

	/// <inheritdoc/>
	[DoesNotReturn]
	readonly void IEnumerator.Reset() => throw new NotImplementedException();
}
