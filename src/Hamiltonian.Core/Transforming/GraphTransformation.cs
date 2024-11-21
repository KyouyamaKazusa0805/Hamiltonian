namespace Hamiltonian.Transforming;

/// <summary>
/// Provides a way to transform a <see cref="Graph"/>.
/// </summary>
/// <seealso cref="Graph"/>
public static class GraphTransformation
{
	/// <summary>
	/// Rotate a graph clockwise.
	/// </summary>
	/// <param name="graph">The graph.</param>
	/// <returns>The result.</returns>
	public static Graph RotateClockwise(this Graph graph)
	{
		var matrix = (bool[,])graph;
		var rows = graph.ColumnsLength;
		var columns = graph.RowsLength;

		// 1 2 3      7 4 1
		// 4 5 6  ->  8 5 2
		// 7 8 9      9 6 3
		var result = new bool[rows, columns];
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < columns; ++j)
			{
				result[i, j] = matrix[columns - j - 1, i];
			}
		}
		return new(result);
	}

	/// <summary>
	/// Rotate a graph counter-clockwise.
	/// </summary>
	/// <param name="graph">The graph.</param>
	/// <returns>The result.</returns>
	public static Graph RotateCounterclockwise(this Graph graph)
	{
		var matrix = (bool[,])graph;
		var rows = graph.ColumnsLength;
		var columns = graph.RowsLength;

		// 1 2 3      3 6 9
		// 4 5 6  ->  2 5 8
		// 7 8 9      1 4 7
		var result = new bool[rows, columns];
		for (int i = 0; i < graph.RowsLength; i++)
		{
			for (int j = 0; j < graph.ColumnsLength; j++)
			{
				result[rows - j - 1, i] = matrix[i, j];
			}
		}
		return new(result);
	}
}
