namespace Hamiltonian.Measuring;

/// <summary>
/// Represents a degree measurer.
/// </summary>
public static class Degree
{
	/// <summary>
	/// Gets the sum of the degree value.
	/// </summary>
	/// <param name="graph">The graph.</param>
	/// <returns>The sum degree value.</returns>
	public static int GetDegreeSum(Graph graph)
	{
		var result = 0;
		foreach (var cell in graph.EnumerateCoordinates())
		{
			if (!cell.Up.IsOutOfBound(graph) && graph[cell.Up])
			{
				result++;
			}
			if (!cell.Down.IsOutOfBound(graph) && graph[cell.Down])
			{
				result++;
			}
			if (!cell.Left.IsOutOfBound(graph) && graph[cell.Left])
			{
				result++;
			}
			if (!cell.Right.IsOutOfBound(graph) && graph[cell.Right])
			{
				result++;
			}
		}
		return result;
	}
}
