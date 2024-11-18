namespace Hamiltonian.Generating;

/// <summary>
/// Represents a generator that can generate a <see cref="Graph"/> instance with a unique path.
/// </summary>
/// <seealso cref="Graph"/>
public readonly ref struct Generator
{
	/// <summary>
	/// Indicates the random number generator.
	/// </summary>
	private readonly Random _random = new();

	/// <summary>
	/// Indicates the backing solver.
	/// </summary>
	private readonly Solver _solver = new();


	/// <summary>
	/// Initializes a <see cref="Generator"/> instance.
	/// </summary>
	/// <param name="rows">The number of rows.</param>
	/// <param name="columns">The number of columns.</param>
	public Generator(int rows, int columns) => (RowsLength, ColumnsLength) = (rows, columns);


	/// <summary>
	/// Indicates the number of rows should be generated.
	/// </summary>
	public int RowsLength { get; }

	/// <summary>
	/// Indicates the number of columns should be generated.
	/// </summary>
	public int ColumnsLength { get; }


	/// <inheritdoc/>
	[DoesNotReturn]
	public override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException();

	/// <inheritdoc/>
	[DoesNotReturn]
	public override int GetHashCode() => throw new NotSupportedException();

	/// <inheritdoc/>
	[DoesNotReturn]
	public override string? ToString() => throw new NotSupportedException();

	/// <summary>
	/// Generate a graph puzzle with a unique path, by using the specified cell as the start.
	/// </summary>
	/// <param name="start">The start position.</param>
	/// <param name="end">The end position.</param>
	/// <param name="path">The uniqueness path of the puzzle.</param>
	/// <param name="cancellationToken">The cancellation token that can cancel the generation.</param>
	/// <returns>The result generated. If canceled, <see langword="null"/> will be returned.</returns>
	[return: NotNullIfNotNull(nameof(path))]
	public Graph? Generate(Coordinate start, out Coordinate end, out Path? path, CancellationToken cancellationToken = default)
	{
		while (true)
		{
			var graph = new Graph(RowsLength, ColumnsLength);
			var coordinates = new Stack<Coordinate>();
			var size = _random.Next(RowsLength * ColumnsLength >> 1, (int)(RowsLength * ColumnsLength * .75));
			try
			{
				dfs(in this, graph, start, start, coordinates, size, cancellationToken);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (InvalidOperationException)
			{
				continue;
			}
			catch
			{
			}

			// Verify the uniqueness of the puzzle.
			if (!_solver.IsValid(graph, start, null, out var resultPath))
			{
				goto NextLoop;
			}

			// Check whether at least one row or column is empty (out of using).
			var flag = true;
			for (var i = 0; i < graph.RowsLength; i++)
			{
				if (graph.SliceRow(i).All(booleanCondition))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				for (var i = 0; i < graph.ColumnsLength; i++)
				{
					if (graph.SliceColumn(i).All(booleanCondition))
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				continue;
			}

			end = coordinates.Peek();
			path = resultPath;
			return graph;

		NextLoop:
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}
		}

		end = default;
		path = null;
		return null;


		static bool booleanCondition(bool element) => !element;

		static void dfs(
			ref readonly Generator @this,
			Graph graph,
			Coordinate start,
			Coordinate current,
			Stack<Coordinate> coordinates,
			int size,
			CancellationToken cancellationToken = default
		)
		{
			graph[current] = true;
			coordinates.Push(current);

			if (!graph.IsEmpty && !@this._solver.IsValid(graph, start, current, out _))
			{
				goto Backtrack;
			}

			if (coordinates.Count >= size)
			{
				throw new();
			}

			// Check validity of four directions.
			var directions = new List<Direction>(4);
			if (!current.Up.IsOutOfBound(graph) && !graph[current.Up])
			{
				directions.Add(Direction.Up);
			}
			if (!current.Down.IsOutOfBound(graph) && !graph[current.Down])
			{
				directions.Add(Direction.Down);
			}
			if (!current.Left.IsOutOfBound(graph) && !graph[current.Left])
			{
				directions.Add(Direction.Left);
			}
			if (!current.Right.IsOutOfBound(graph) && !graph[current.Right])
			{
				directions.Add(Direction.Right);
			}
			if (directions.Count == 0)
			{
				// No directions available.
				goto Backtrack;
			}

			var nextDirection = directions[@this._random.Next(0, directions.Count)];
			dfs(in @this, graph, start, current >> nextDirection, coordinates, size, cancellationToken);

		Backtrack:
			// If here, the path won't be okay.
			graph[current] = false;
			coordinates.Pop();
			cancellationToken.ThrowIfCancellationRequested();
		}
	}
}
