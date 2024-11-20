namespace Hamiltonian;

/// <summary>
/// Represents a Hamiltonian graph.
/// </summary>
public sealed partial class Graph :
	ICloneable,
	IEquatable<Graph>,
	IEqualityOperators<Graph, Graph, bool>,
	IFormattable,
	IParsable<Graph>,
	IReadOnlyCollection<bool>,
	IReadOnlyList<bool>
{
	/// <summary>
	/// Indicates the sequence of the graph.
	/// </summary>
	private readonly BitArray _sequence;


	/// <summary>
	/// Initializes a <see cref="Graph"/> instance via the specified number of rows and columns,
	/// treating all cells as "off" state. 
	/// </summary>
	/// <param name="rows">Indicates the number of rows.</param>
	/// <param name="columns">Indicates the number of columns.</param>
	public Graph(int rows, int columns)
	{
		(RowsLength, ColumnsLength, _sequence) = (rows, columns, new(rows * columns));
		_sequence.SetAll(false);
	}

	/// <summary>
	/// Copies a list of bits from the specified bit array.
	/// </summary>
	/// <param name="bitArray">The bit array.</param>
	private Graph(BitArray bitArray) => _sequence = (BitArray)bitArray.Clone();


	/// <summary>
	/// Indicates whether the graph is empty.
	/// </summary>
	public bool IsEmpty => Length == 0;

	/// <summary>
	/// Indicates the number of cells used.
	/// </summary>
	public int Length => _sequence.GetCardinality();

	/// <summary>
	/// Indicates the size of the graph (the number of cells used).
	/// </summary>
	public int Size => _sequence.Count;

	/// <summary>
	/// Indicates the number of rows used.
	/// </summary>
	public int RowsLength { get; }

	/// <summary>
	/// Indicates the number of columns used.
	/// </summary>
	public int ColumnsLength { get; }

	/// <inheritdoc/>
	int IReadOnlyCollection<bool>.Count => Length;


	[GeneratedRegex("""^(\d+):(\d+)(:([01]{4,}))?$""", RegexOptions.Compiled)]
	private static partial Regex ParsingFormatPattern { get; }

	[GeneratedRegex("1", RegexOptions.Compiled)]
	private static partial Regex OnStatePattern { get; }


	/// <summary>
	/// Gets or sets the state at the specified index.
	/// </summary>
	/// <param name="index">The desired index.</param>
	/// <returns>The boolean state to get or set.</returns>
	public bool this[int index]
	{
		get => _sequence[index];

		set => _sequence[index] = value;
	}

	/// <inheritdoc cref="this[int]"/>
	public bool this[Coordinate index]
	{
		get => _sequence[index.ToIndex(this)];

		set => _sequence[index.ToIndex(this)] = value;
	}


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as Graph);

	/// <inheritdoc/>
	public bool Equals([NotNullWhen(true)] Graph? other)
		=> other is not null && _sequence.SequenceEqual(other._sequence);

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		var length = (_sequence.Count & 7) == 0 ? _sequence.Count >> 3 : (_sequence.Count >> 3) + 1;
		var byteSequence = new byte[length];
		_sequence.CopyTo(byteSequence, 0);

		var hashCode = new HashCode();
		hashCode.AddBytes(byteSequence);
		return hashCode.ToHashCode();
	}

	/// <summary>
	/// Gets the degree value at the specified coordinate.
	/// </summary>
	/// <param name="coordinate">The coordinate.</param>
	/// <returns>The degree value. The return value must be 2, 3 or 4.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetDegreeAt(Coordinate coordinate)
	{
		var result = 0;
		if (!coordinate.Up.IsOutOfBound(this) && this[coordinate.Up])
		{
			result++;
		}
		if (!coordinate.Down.IsOutOfBound(this) && this[coordinate.Down])
		{
			result++;
		}
		if (!coordinate.Left.IsOutOfBound(this) && this[coordinate.Left])
		{
			result++;
		}
		if (!coordinate.Right.IsOutOfBound(this) && this[coordinate.Right])
		{
			result++;
		}
		return result;
	}

	/// <inheritdoc/>
	public override string ToString() => ToString("b");

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(string? format)
	{
		switch (format)
		{
			case null or "b":
			{
				var charSequence = (stackalloc char[Size]);
				for (var i = 0; i < Size; i++)
				{
					charSequence[i] = _sequence[i] ? '1' : '0';
				}
				return charSequence.ToString();
			}
			case "bs":
			{
				var charSequence = (stackalloc char[Size]);
				for (var i = 0; i < Size; i++)
				{
					charSequence[i] = _sequence[i] ? '1' : '0';
				}
				return $"{RowsLength}:{ColumnsLength}:{charSequence}";
			}
			case "c":
			{
				const char used = '#', unused = ' ';
				var sb = new StringBuilder();
				for (var i = 0; i < Size; i++)
				{
					sb.Append(_sequence[i] ? used : unused);
					if ((i + 1) % ColumnsLength == 0)
					{
						sb.AppendLine();
					}
				}
				return sb.RemoveFrom(^Environment.NewLine.Length).ToString();
			}
			default:
			{
				throw new FormatException();
			}
		}
	}

	/// <summary>
	/// Slices a row, and integrates the values into a sequence of <see cref="bool"/> values.
	/// </summary>
	/// <param name="row">The desired row label.</param>
	/// <returns>A list of <see cref="bool"/> result sliced.</returns>
	public ReadOnlySpan<bool> SliceRow(int row)
	{
		var result = new bool[ColumnsLength];
		var startIndex = row * ColumnsLength;
		for (var i = 0; i < ColumnsLength; i++)
		{
			result[i] = _sequence[startIndex++];
		}
		return result;
	}

	/// <summary>
	/// Slices a column, and integrates the values into a sequence of <see cref="bool"/> values.
	/// </summary>
	/// <param name="column">The desired column label.</param>
	/// <returns>A list of <see cref="bool"/> result sliced.</returns>
	public ReadOnlySpan<bool> SliceColumn(int column)
	{
		var result = new bool[RowsLength];
		var startIndex = column;
		for (var i = 0; i < RowsLength; i++, startIndex += ColumnsLength)
		{
			result[i] = _sequence[startIndex];
		}
		return result;
	}

	/// <summary>
	/// Returns an enumerator that iterates on each bits of the sequence.
	/// </summary>
	/// <returns>An enumerator that iterates on each bits of the sequence.</returns>
	public Enumerator GetEnumerator() => new(_sequence);

	/// <summary>
	/// Returns an enumerator that iterates on each coordinates of cells used.
	/// </summary>
	/// <returns>An enumerator that iterates on each coordinates of cells used.</returns>
	public CoordinateEnumerator EnumerateCoordinates() => new(_sequence, ColumnsLength);

	/// <inheritdoc cref="ICloneable.Clone"/>
	public Graph Clone() => new(_sequence);

	/// <summary>
	/// Returns a read-only bit array.
	/// </summary>
	/// <returns>The bit array.</returns>
	public BitArray ToBitArray() => (BitArray)_sequence.Clone();

	/// <inheritdoc/>
	object ICloneable.Clone() => Clone();

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format);

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<bool>)this).GetEnumerator();

	/// <inheritdoc/>
	IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
	{
		for (var i = 0; i < Size; i++)
		{
			yield return _sequence[i];
		}
	}


	/// <summary>
	/// Generates an empty graph with the specified number of rows and columns.
	/// </summary>
	/// <param name="rows">The rows.</param>
	/// <param name="columns">The columns.</param>
	/// <returns>A graph.</returns>
	public static Graph Empty(int rows, int columns) => Parse($"{rows}:{columns}");

	/// <inheritdoc cref="Parse(string)"/>
	public static Graph Parse(ReadOnlySpan<char> s) => Parse(s.ToString());

	/// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)"/>
	/// <remarks>
	/// <para>
	/// Match format:
	/// <code><![CDATA[<row>:<column>:<data>]]></code>
	/// </para>
	/// <para>
	/// Meaning:
	/// <list type="bullet">
	/// <item><c>row</c>: The number of rows is <c>row</c>.</item>
	/// <item><c>column</c>: The number of columns is <c>column</c>.</item>
	/// <item><c>data</c>: Detail of on/off graph sequence (Use characters <c>'0'</c> and <c>'1'</c>; 0 - used, 1 - unused).</item>
	/// <item><c>colon token ':'</c>: Separator.</item>
	/// </list>
	/// </para>
	/// <para><c>data</c> can be omitted. If <c>data</c> is omitted, the generated graph will set all cells "off".</para>
	/// </remarks>
	public static Graph Parse(string s)
	{
		switch (ParsingFormatPattern.Match(s))
		{
			case { Success: true, Groups: [_, { Value: var a }, { Value: var b }, _, { Value: var c }] }:
			{
				if (c == string.Empty)
				{
					return new(int.Parse(a), int.Parse(b));
				}

				var result = new Graph(int.Parse(a), int.Parse(b));
				foreach (var match in OnStatePattern.EnumerateMatches(c))
				{
					result[match.Index] = true;
				}
				return result;
			}
			default:
			{
				throw new FormatException();
			}
		}
	}

	/// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/>
	public static bool TryParse(string? s, [NotNullWhen(true)] out Graph? result)
	{
		try
		{
			if (s is null)
			{
				throw new FormatException();
			}

			result = Parse(s);
			return true;
		}
		catch (FormatException)
		{
			result = null;
			return false;
		}
	}

	/// <inheritdoc/>
	static bool IParsable<Graph>.TryParse(string? s, IFormatProvider? provider, [NotNullWhen(true)] out Graph? result)
		=> TryParse(s, out result);

	/// <inheritdoc/>
	static Graph IParsable<Graph>.Parse(string s, IFormatProvider? provider) => Parse(s);


	/// <inheritdoc/>
	public static bool operator ==(Graph? left, Graph? right)
		=> (left, right) switch
		{
			(null, null) => true,
			(not null, not null) => left.Equals(right),
			_ => false
		};

	/// <inheritdoc/>
	public static bool operator !=(Graph? left, Graph? right) => !(left == right);

	/// <summary>
	/// Adds a new coordinate into the graph.
	/// </summary>
	/// <param name="base">The base graph.</param>
	/// <param name="coordinate">The coordinate.</param>
	/// <returns>The new graph with the specified coordinate added.</returns>
	public static Graph operator +(Graph @base, Coordinate coordinate)
	{
		var result = @base.Clone();
		result[coordinate] = true;
		return result;
	}

	/// <summary>
	/// Removes a coordinate from the graph.
	/// </summary>
	/// <param name="base">The base graph.</param>
	/// <param name="coordinate">The coordinate.</param>
	/// <returns>The new graph with the specified coordinate removed.</returns>
	public static Graph operator -(Graph @base, Coordinate coordinate)
	{
		var result = @base.Clone();
		result[coordinate] = false;
		return result;
	}
}
