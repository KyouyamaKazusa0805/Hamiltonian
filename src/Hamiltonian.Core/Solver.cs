namespace Hamiltonian;

/// <summary>
/// Provides a way to verify validity of a Hamiltonian graph.
/// </summary>
public sealed class Solver
{
	/// <summary>
	/// Determine whether the specified graph has a unique path.
	/// </summary>
	/// <param name="graph">Indicates the graph.</param>
	/// <param name="result">Indicates the result of the path.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	public bool IsValid(Graph graph, [NotNullWhen(true)] out Path? result)
	{
		if (graph == Graph.Empty(graph.RowsLength, graph.ColumnsLength))
		{
			// No cells chosen.
			result = null;
			return false;
		}

		foreach (var cell in graph.EnumerateCoordinates())
		{
			if (IsValid(graph, cell, null, out result))
			{
				return true;
			}
		}

		result = null;
		return false;
	}

	/// <summary>
	/// Determine whether the specified graph has a unique path, from the specified cell as the start,
	/// to the specified cell as the end.
	/// </summary>
	/// <param name="graph">Indicates the graph.</param>
	/// <param name="start">The start cell.</param>
	/// <param name="end">The end cell. The end cell can be <see langword="null"/> if not specified.</param>
	/// <param name="result">Indicates the result of the path.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	public bool IsValid(Graph graph, Coordinate start, Coordinate? end, [NotNullWhen(true)] out Path? result)
	{
		if (graph == Graph.Empty(graph.RowsLength, graph.ColumnsLength))
		{
			// No cells chosen.
			result = null;
			return false;
		}

		if (graph.Length == 1)
		{
			if (graph[start])
			{
				result = new(start);
				return true;
			}
			else
			{
				result = null;
				return false;
			}
		}

		var solutions = new List<Path>(2);
		try
		{
			dfs(start, new(start), new(graph.Size), solutions);
		}
		catch
		{
		}

		if (solutions is [var solution])
		{
			result = solution;
			return true;
		}
		else
		{
			result = null;
			return false;
		}


		void dfs(
			Coordinate start,
			CoordinateNode currentNode,
			BitArray bits,
			List<Path> solutions
		)
		{
			var currentCoordinate = currentNode.Coordinate;
			bits[currentCoordinate.ToIndex(graph)] = true;

			var path = new Path(currentNode);
			if (path.AsGraph(graph.RowsLength, graph.ColumnsLength) == graph
				&& (end is { } realEnd && realEnd == currentCoordinate || end is null))
			{
				// Finished.
				solutions.Add(path);
				if (solutions.Count >= 2)
				{
					throw new();
				}
			}

			// Find adjacent cells for all four cells.
			if (currentCoordinate.Up is var up && !up.IsOutOfBound(graph) && graph[up] && !bits[up.ToIndex(graph)])
			{
				dfs(start, new(up, currentNode), bits, solutions);
			}
			if (currentCoordinate.Down is var down && !down.IsOutOfBound(graph) && graph[down] && !bits[down.ToIndex(graph)])
			{
				dfs(start, new(down, currentNode), bits, solutions);
			}
			if (currentCoordinate.Left is var left && !left.IsOutOfBound(graph) && graph[left] && !bits[left.ToIndex(graph)])
			{
				dfs(start, new(left, currentNode), bits, solutions);
			}
			if (currentCoordinate.Right is var right && !right.IsOutOfBound(graph) && graph[right] && !bits[right.ToIndex(graph)])
			{
				dfs(start, new(right, currentNode), bits, solutions);
			}

			// Backtracking.
			bits[currentCoordinate.ToIndex(graph)] = false;
		}
	}
}
