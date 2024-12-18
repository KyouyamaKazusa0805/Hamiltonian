﻿namespace Hamiltonian.Concepts;

/// <summary>
/// Represents a linked node for a coordinate.
/// </summary>
internal sealed class CoordinateNode(Coordinate coordinate, CoordinateNode? parent)
{
	/// <summary>
	/// Initializes a <see cref="CoordinateNode"/> instance.
	/// </summary>
	/// <param name="coordinate">The coordinate.</param>
	public CoordinateNode(Coordinate coordinate) : this(coordinate, null)
	{
	}


	/// <summary>
	/// Indicates the coordinate.
	/// </summary>
	public Coordinate Coordinate { get; } = coordinate;

	/// <summary>
	/// Indicates the parent node.
	/// </summary>
	public CoordinateNode? Parent { get; } = parent;

	/// <summary>
	/// Indicates the root node.
	/// </summary>
	public CoordinateNode Root
	{
		get
		{
			var result = this;
			var p = Parent;
			while (p is not null)
			{
				result = p;
				p = p.Parent;
			}
			return result;
		}
	}


	/// <inheritdoc/>
	public override string ToString()
		=> $$"""{{nameof(CoordinateNode)}} { {{nameof(Coordinate)}} = {{Coordinate}}, {{nameof(Parent)}} = {{Parent?.Coordinate.ToString() ?? "<null>"}} }""";
}
