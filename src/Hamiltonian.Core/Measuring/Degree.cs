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

	/// <summary>
	/// Gets a dictionary that describes the times of appearing of nodes of the specified degree.
	/// </summary>
	/// <param name="graph">The graph.</param>
	/// <returns>The dictionary that describes the times of appearing.</returns>
	public static FrozenDictionary<int, int> GetDegreeFrequency(Graph graph)
	{
		var result = new Dictionary<int, int>(4);
		foreach (var cell in graph.EnumerateCoordinates())
		{
			var times = 0;
			if (!cell.Up.IsOutOfBound(graph) && graph[cell.Up])
			{
				times++;
			}
			if (!cell.Down.IsOutOfBound(graph) && graph[cell.Down])
			{
				times++;
			}
			if (!cell.Left.IsOutOfBound(graph) && graph[cell.Left])
			{
				times++;
			}
			if (!cell.Right.IsOutOfBound(graph) && graph[cell.Right])
			{
				times++;
			}
			CollectionsMarshal.GetValueRefOrAddDefault(result, times, out _)++;
		}
		return result.ToFrozenDictionary();
	}
}
