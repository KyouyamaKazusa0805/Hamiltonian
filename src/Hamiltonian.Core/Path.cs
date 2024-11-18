﻿namespace Hamiltonian;

/// <summary>
/// Provides a path of a Hamiltonian graph.
/// </summary>
public sealed class Path :
	IEquatable<Path>,
	IEqualityOperators<Path, Path, bool>,
	IReadOnlyCollection<Coordinate>,
	IReadOnlyList<Coordinate>
{
	/// <summary>
	/// Indicates the coordinates.
	/// </summary>
	private readonly Coordinate[] _coordinates;


	/// <summary>
	/// Initializes a <see cref="Path"/> instance via the specified path.
	/// </summary>
	/// <param name="coordinates">The coordinates.</param>
	public Path(params Coordinate[] coordinates) => _coordinates = coordinates;

	/// <summary>
	/// Initializes a <see cref="Path"/> instance via the specified node as the last node.
	/// </summary>
	/// <param name="lastNode">The last node.</param>
	internal Path(CoordinateNode lastNode)
	{
		var nodes = new Stack<Coordinate>();
		for (var node = lastNode; node is not null; node = node.Parent)
		{
			nodes.Push(node.Coordinate);
		}
		_coordinates = [.. nodes];
	}


	/// <summary>
	/// Indicates the length of the coordinates used.
	/// </summary>
	public int Length => _coordinates.Length;

	/// <summary>
	/// Returns the backing elements, integrated as a <see cref="ReadOnlySpan{T}"/>.
	/// </summary>
	public ReadOnlySpan<Coordinate> Span => _coordinates;

	/// <inheritdoc/>
	int IReadOnlyCollection<Coordinate>.Count => Length;


	/// <summary>
	/// Gets the coordinate at the specified index.
	/// </summary>
	/// <param name="index">The desired index.</param>
	/// <returns>The coordinate.</returns>
	/// <exception cref="IndexOutOfRangeException">Throws when the index is out of range.</exception>
	public Coordinate this[int index] => _coordinates[index];


	/// <inheritdoc/>
	public override bool Equals(object? obj) => Equals(obj as Path);

	/// <inheritdoc/>
	public bool Equals([NotNullWhen(true)] Path? other)
		=> other is not null && _coordinates.AsSpan().SequenceEqual(other._coordinates);

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		var result = new HashCode();
		foreach (var element in _coordinates)
		{
			result.Add(element);
		}
		return result.ToHashCode();
	}

	/// <inheritdoc/>
	public override string ToString() => ToString(" -> ");

	/// <summary>
	/// Converts the current instance into a <see cref="string"/> result.
	/// </summary>
	/// <param name="separator">The separator.</param>
	/// <returns>The string result.</returns>
	public string ToString(string separator)
	{
		var sb = new StringBuilder();
		foreach (var element in _coordinates)
		{
			sb.Append(element.ToString());
			sb.Append(separator);
		}
		return sb.RemoveFrom(^separator.Length).ToString();
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	public ArrayEnumerator<Coordinate> GetEnumerator() => new(_coordinates);

	/// <summary>
	/// Converts the current path object into a <see cref="Graph"/> instance.
	/// </summary>
	/// <param name="rows">The number of rows.</param>
	/// <param name="columns">The number of columns.</param>
	/// <returns>A <see cref="Graph"/> result casted.</returns>
	public Graph AsGraph(int rows, int columns)
	{
		var result = new Graph(rows, columns);
		foreach (var coordinate in _coordinates)
		{
			result[coordinate] = true;
		}
		return result;
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Coordinate>)this).GetEnumerator();

	/// <inheritdoc/>
	IEnumerator<Coordinate> IEnumerable<Coordinate>.GetEnumerator() => ((IEnumerable<Coordinate>)_coordinates).GetEnumerator();


	/// <inheritdoc/>
	public static bool operator ==(Path? left, Path? right)
		=> (left, right) switch
		{
			(null, null) => true,
			(not null, not null) => left.Equals(right),
			_ => false
		};

	/// <inheritdoc/>
	public static bool operator !=(Path? left, Path? right) => !(left == right);
}
